using System;
using System.Collections.Generic;
using Dajjsand.Enums;
using Dajjsand.Factories.CardFactory;
using Dajjsand.View.Game.Cards;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Dajjsand.Tests
{
    public class TestCardsFromPackPlacement : MonoBehaviour
    {
        [SerializeField] private int _cardsCount;
        [SerializeField] private float _range;
        [SerializeField] private int _cardsInFirstRound;
        [SerializeField] private int _cardsIncreaseInEachRound;
        [SerializeField, ReadOnly] private int _currentCardIndex;

        private List<BaseCard> _cards = new List<BaseCard>();
        private ICardFactory _factory;

        [Inject]
        private void Construct(ICardFactory factory)
        {
            _factory = factory;
        }

        private void Start()
        {
            ResetCount();
        }

        [Button("Spawn Card")]
        private void OnMouseUpAsButton()
        {
            var newCard = _factory.GetCard(CardType.Coin, transform.position + CardOffset(_currentCardIndex, _cardsInFirstRound, _cardsIncreaseInEachRound, _cardsCount));
            _currentCardIndex++;
            _cards.Add(newCard);
        }

        private Vector3 CardOffset(int cardIndex, int cardsInFirstRound, int cardsIncreaseInEachRound, int totalCardsCount)
        {
            int currentCardRound = 0;
            int cardsInRound = cardsInFirstRound + cardsIncreaseInEachRound * currentCardRound;
            while (cardIndex >= cardsInRound)
            {
                currentCardRound++;
                cardIndex -= cardsInRound;
                cardsInRound = cardsInFirstRound + cardsIncreaseInEachRound * currentCardRound;
            }

            float angle = (360f / cardsInRound) * cardIndex;
            Vector3 vector = new Vector3(_range * (currentCardRound + 1), 0, 0);
            vector = Quaternion.AngleAxis(angle, Vector3.up) * vector;
            vector.Scale(new Vector3(0.7f, 1f, 1f));

            return vector;
        }

        [Button("Reset Count")]
        private void ResetCount()
        {
            _currentCardIndex = 0;
            foreach (BaseCard card in _cards)
                _factory.ReleaseCard(card);

            _cards.Clear();
        }
    }
}