using System;
using System.Collections.Generic;
using Dajjsand.Enums;
using UnityEngine;

namespace Dajjsand.View.Game.Cards
{
    public class BaseCard : MonoBehaviour
    {
        public event Action<BaseCard> OnClick;

        [SerializeField] private Renderer _renderer;
        [SerializeField] private int _numberOfRemainingUses = 1;

        public CardType IngredientType { get; private set; }
        public bool IsDraggingLocked { get; set; }

        private Dictionary<CardType, int> _ingredients;

        public void Init(CardType ingredientType, Texture texture)
        {
            IngredientType = ingredientType;
            _renderer.material.mainTexture = texture;
            _ingredients = new();
        }

        public virtual void Used()
        {
            _numberOfRemainingUses--;
            if (_numberOfRemainingUses <= 0)
                Destroy(gameObject);
        }

        public void SetIngredients(Dictionary<CardType, int> ingredients)
        {
            _ingredients = ingredients;
        }

        public CardType? GetCardFromContainer()
        {
            foreach (CardType card in _ingredients.Keys)
            {
                if (_ingredients[card] > 0)
                {
                    _ingredients[card]--;
                    if (_ingredients[card] == 0)
                        _ingredients.Remove(card);

                    return card;
                }
            }

            return null;
        }

        public bool IsAnyCardInContainer() => _ingredients.Count > 0;

        private void OnMouseUpAsButton() => OnClick?.Invoke(this);
    }
}