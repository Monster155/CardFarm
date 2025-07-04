﻿using System;
using System.Collections.Generic;
using System.Timers;
using Dajjsand.Controllers.Craft;
using Dajjsand.Enums;
using Dajjsand.ScriptableObjects;
using DG.Tweening;

namespace Dajjsand.View.Game.Cards
{
    public class CardLogic
    {
        public event Action OnUsesCountEnd;
        public event Action<BaseCard> OnParentChanged;
        public event Action<float> OnMergeTimerUpdate;
        public event Action OnMergeTimerStart;
        public event Action OnMergeTimerStop;
        public event Action OnMergeTimerFinish;

        // base data
        public CardData CardData { get; private set; }
        public BaseCard Card { get; private set; }

        // parenting
        public CardLogic HeadCard { get; private set; }
        public CardLogic ChildCard { get; private set; }

        // logic data
        private Dictionary<CardType, int> _cardsInside;
        private int _numberOfRemainingUses;

        // rules
        public CardType Type => CardData._cardType;
        private Tweener _mergeTimer;


        public void Init(CardData cardData, BaseCard card)
        {
            CardData = cardData;
            _numberOfRemainingUses = CardData._numberOfUses;

            Card = card;
            HeadCard = this;

            _cardsInside = new();
        }

        public virtual void Used()
        {
            if (_numberOfRemainingUses < 0)
                return;

            _numberOfRemainingUses--;
            if (_numberOfRemainingUses == 0)
                OnUsesCountEnd?.Invoke();
        }

        #region CardsContainer

        public void SetCardToContainer(Dictionary<CardType, int> cards)
        {
            _cardsInside = cards;
        }

        public CardType? GetCardFromContainer()
        {
            foreach (CardType card in _cardsInside.Keys)
            {
                if (_cardsInside[card] > 0)
                {
                    _cardsInside[card]--;
                    if (_cardsInside[card] == 0)
                        _cardsInside.Remove(card);

                    return card;
                }
            }

            return null;
        }

        public bool IsAnyCardInContainer() => _cardsInside.Count > 0;

        #endregion


        public void MakeParenting(CardLogic childHighestCard)
        {
            var lowestCard = this;
            while (lowestCard.ChildCard != null)
            {
                lowestCard = lowestCard.ChildCard;
            }

            lowestCard.ChildCard = childHighestCard;

            var card = childHighestCard;
            card.HeadCard = HeadCard;
            while (card.ChildCard != null)
            {
                card = card.ChildCard;
                card.HeadCard = HeadCard;
            }

            childHighestCard.OnParentChanged?.Invoke(lowestCard.Card);

            StartTimer();
        }

        private void StartTimer()
        {
            if (CraftController.Instance.TryToStartMerge(
                    HeadCard,
                    percentage => OnMergeTimerUpdate?.Invoke(percentage),
                    MergeTimerFinish,
                    out Tweener timer))
            {
                OnMergeTimerStart?.Invoke();
                _mergeTimer = timer;
            }
        }

        private void MergeTimerFinish()
        {
            OnMergeTimerFinish?.Invoke();
            StartTimer();
        }

        public void LoseChildren()
        {
            HeadCard.StopMergeTimer();

            if (ChildCard != null)
                ChildCard.HeadCard = ChildCard;
            ChildCard = null;

            OnParentChanged?.Invoke(null);
        }

        private void StopMergeTimer()
        {
            _mergeTimer?.Kill();
            OnMergeTimerStop?.Invoke();
        }
    }
}