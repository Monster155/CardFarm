using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Dajjsand.Controllers.Craft;
using Dajjsand.Enums;
using Dajjsand.ScriptableObjects;
using Dajjsand.Utils.Constants;
using Dajjsand.Utils.Logic;
using DG.Tweening;
using UnityEngine;

namespace Dajjsand.View.Game.Cards
{
    public class CardLogic : MonoBehaviour
    {
        public event Action<int> OnUsesCountChanged;
        public event Action<CardLogic> OnParentChanged;

        public CardType Type => _cardData._cardType;

        // parenting
        public CardLogic HeadCard { get; private set; }
        public CardLogic ParentCard { get; private set; }
        public CardLogic ChildCard { get; private set; }

        public int DeckSize { get; private set; } = 1;
        public int ID { get; private set; }

        // serialize fields
        [SerializeField] private MergeBar _mergeBar;
        [field: SerializeField] public Transform ChildContainer { get; private set; }

        // logic data
        private CardData _cardData;

        private int _numberOfRemainingUses;

        private Dictionary<CardType, int> _cardsInside;
        private int _cardsDroppedFromPack;

        private Tweener _mergeTimer;


        public void Init(CardData cardData, int id)
        {
            _cardData = cardData;
            ID = id;
            _numberOfRemainingUses = _cardData._numberOfUses;

            HeadCard = this;
            ParentCard = null;
            ChildCard = null;

            _cardsInside = new();
            _cardsDroppedFromPack = 0;
        }

        public void Used()
        {
            if (_numberOfRemainingUses < 0)
                return;

            _numberOfRemainingUses--;
            OnUsesCountChanged?.Invoke(_numberOfRemainingUses);
        }

        public void ReleasingCard()
        {
            // lowest card
            // do nothing

            // middle card
            if (ParentCard != null && ChildCard != null)
            {
                ParentCard.ChildCard = this.ChildCard;
                this.ChildCard.OnParentChanged?.Invoke(ParentCard);
            }

            // highest card
            if (HeadCard.ID == this.ID)
            {
                if (ChildCard != null)
                {
                    ChildCard.ParentCard = null;
                    ChildCard.OnParentChanged?.Invoke(null);
                    ChildCard.SetThisCardAsNewHeadCard(ChildCard);
                }
                // if it deck from 1 card - do nothing
            }
        }

        #region CardsContainer

        public void SetCardToContainer(Dictionary<CardType, int> cards)
        {
            _cardsInside = cards
                .Where(pair => pair.Value > 0)
                .ToDictionary(kv => kv.Key, kv => kv.Value);
            _cardsDroppedFromPack = 0;
        }

        public CardType? GetCardFromContainer(out Vector3 offset)
        {
            offset = CardUtils.CardOffset(_cardsDroppedFromPack);

            if (_cardsInside.Count == 0)
                return null;

            int rand = UnityEngine.Random.Range(0, _cardsInside.Count);
            CardType card = _cardsInside.Keys.ElementAt(rand);

            _cardsInside[card]--;
            if (_cardsInside[card] == 0)
                _cardsInside.Remove(card);

            _cardsDroppedFromPack++; // increase index only if card had
            return card;
        }

        public bool IsAnyCardInContainer() => _cardsInside.Count > 0;

        #endregion

        #region ParentingAndMerge

        public void AddCardsFrom(CardLogic newDeckCard)
        {
            // find the lowest card
            var currentDeckLowestCard = this;
            while (currentDeckLowestCard.ChildCard != null)
                currentDeckLowestCard = currentDeckLowestCard.ChildCard;

            // set transform parent of decks
            newDeckCard.HeadCard.OnParentChanged?.Invoke(currentDeckLowestCard);
            
            newDeckCard.ParentCard = currentDeckLowestCard;
            currentDeckLowestCard.ChildCard = newDeckCard.HeadCard; // add cards from new deck to bottom
            SetThisCardAsNewHeadCard(newDeckCard); // set new HeadCard to all cards from new deck

            TryToStartMerge();
        }

        public void LoseParentDeck()
        {
            // all deck took
            if (ParentCard == null)
                return;

            HeadCard.StopMergeTimer();

            ParentCard.ChildCard = null;
            this.ParentCard = null;

            HeadCard = this;
            SetThisCardAsNewHeadCard(this);

            OnParentChanged?.Invoke(null);
        }

        private void SetThisCardAsNewHeadCard(CardLogic anyCardOfDeck)
        {
            var secondDeckCard = anyCardOfDeck.HeadCard;
            secondDeckCard.HeadCard = HeadCard;
            while (secondDeckCard.ChildCard != null)
            {
                secondDeckCard = secondDeckCard.ChildCard;
                secondDeckCard.HeadCard = HeadCard;
            }
        }

        private void TryToStartMerge()
        {
            if (CraftController.Instance.TryToStartMergeCardsInDeck(
                    HeadCard,
                    percentage => _mergeBar.UpdateProgress(percentage),
                    MergeTimerFinish,
                    out Tweener timer))
            {
                _mergeBar.StartMerge();
                _mergeTimer = timer;
            }
        }

        private void MergeTimerFinish()
        {
            _mergeBar.FinishMerge();
            TryToStartMerge();
        }

        private void StopMergeTimer()
        {
            _mergeTimer?.Kill();
            _mergeBar.StopMerge();
        }

        #endregion

        public bool IsMaxSizeDeck(int otherDeckSize)
        {
            Debug.LogWarning("Max Deck Size doesn't work");
            return DeckSize + otherDeckSize > BaseValues.MaxDeckSize;
        }
    }
}