using System;
using System.Collections.Generic;
using Dajjsand.Enums;
using Dajjsand.ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dajjsand.View.Game.Cards
{
    public class BaseCard : MonoBehaviour
    {
        public event Action<BaseCard> OnClick;

        [SerializeField] private Renderer _renderer;
        [SerializeField, ReadOnly] private int _numberOfRemainingUses;

        public CardType IngredientType { get; private set; }
        public bool IsDraggingLocked { get; set; }

        private CardData _cardData;
        private Dictionary<CardType, int> _cardsInside;

        public void Init(CardData cardData)
        {
            _cardData = cardData;

            IngredientType = _cardData._cardType;
            _renderer.material.mainTexture = _cardData._cardTexture;
            _numberOfRemainingUses = _cardData._numberOfUses;

            _cardsInside = new();
        }

        public virtual void Used()
        {
            if (_numberOfRemainingUses < 0)
                return;

            _numberOfRemainingUses--;
            if (_numberOfRemainingUses == 0)
                Destroy(gameObject);
        }

        public void SetIngredients(Dictionary<CardType, int> ingredients)
        {
            _cardsInside = ingredients;
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

        private void OnMouseUpAsButton() => OnClick?.Invoke(this);
    }
}