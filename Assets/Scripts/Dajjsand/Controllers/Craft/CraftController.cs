using System;
using System.Collections.Generic;
using System.Linq;
using Dajjsand.Enums;
using Dajjsand.Factories.CardFactory;
using Dajjsand.ScriptableObjects;
using Dajjsand.View.Game.Cards;
using DG.Tweening;
using UnityEngine;

namespace Dajjsand.Controllers.Craft
{
    public class CraftController
    {
        public static CraftController Instance { get; private set; }

        private Dictionary<CardType, List<CraftRecipe>> _recipeDictionary;
        private ICardFactory _cardFactory;

        public CraftController(List<CraftRecipe> availableRecipes, ICardFactory cardFactory)
        {
            if (Instance != null)
            {
                Debug.LogError("Use CraftController.Instance instead of creating a new one");
                return;
            }

            Instance = this;

            _cardFactory = cardFactory;

            _recipeDictionary = new();
            foreach (CraftRecipe recipe in availableRecipes)
            {
                foreach (var ingredient in recipe._ingredients.Keys)
                {
                    if (!_recipeDictionary.TryGetValue(ingredient, out var recipes))
                    {
                        recipes = new();
                        _recipeDictionary.Add(ingredient, recipes);
                    }

                    recipes.Add(recipe);
                }
            }
        }

        public void Dispose()
        {
            Instance = null;
        }

        public bool CanBeMerged(CardType ingredient1, CardType ingredient2)
        {
            if (_recipeDictionary.TryGetValue(ingredient1, out var recipes1)
                && _recipeDictionary.TryGetValue(ingredient2, out var recipes2))
            {
                // Используем HashSet для быстрого поиска
                HashSet<CraftRecipe> set = new HashSet<CraftRecipe>(recipes1);

                foreach (var item in recipes2)
                {
                    if (set.Contains(item))
                        return item;
                }
            }

            return false;
        }

        public CraftRecipe GetRecipeForStock(Dictionary<CardType, int> ingredientsInHands)
        {
            List<CraftRecipe> recipes = null;
            foreach (CardType type in ingredientsInHands.Keys)
            {
                if (_recipeDictionary.TryGetValue(type, out var r))
                {
                    if (recipes == null || recipes.Count > r.Count)
                        recipes = r;
                }
                else
                    return null;
            }

            if (recipes == null) return null;

            foreach (CraftRecipe recipe in recipes)
            {
                if (recipe._ingredients.Count != ingredientsInHands.Count)
                    continue;

                bool isAllIngredientsSame = true;
                foreach ((CardType type, int requiredCount) in recipe._ingredients)
                {
                    if (!ingredientsInHands.TryGetValue(type, out int availableCount) || requiredCount != availableCount)
                    {
                        isAllIngredientsSame = false;
                        break;
                    }
                }

                if (isAllIngredientsSame)
                    return recipe;
            }

            return null;
        }

        private Dictionary<CardType, int> GetAllCardsInStock(CardLogic headCard)
        {
            Dictionary<CardType, int> ingredients = new();
            var card = headCard;
            ingredients.Add(card.Type, 1);
            while (card.ChildCard != null)
            {
                card = card.ChildCard;
                if (!ingredients.TryAdd(card.Type, 1))
                    ingredients[card.Type]++;
            }

            return ingredients;
        }

        public bool TryToStartMerge(CardLogic headCard, TweenCallback<float> onUpdate, TweenCallback onFinish, out Tweener timer)
        {
            timer = null;
            Dictionary<CardType, int> ingredients = GetAllCardsInStock(headCard);
            var recipe = GetRecipeForStock(ingredients);

            bool isStarted = recipe != null;
            if (isStarted)
                timer = StartMergeTimer(recipe, headCard, onUpdate, onFinish);

            return isStarted;
        }

        private Tweener StartMergeTimer(CraftRecipe recipe, CardLogic headCard, TweenCallback<float> onUpdate, TweenCallback onFinish)
        {
            var timer = DOVirtual.Float(0f, 1f, recipe._craftTime, onUpdate);
            timer.onComplete += () =>
            {
                var card = headCard;
                card.Used();
                while (card.ChildCard != null)
                {
                    card = card.ChildCard;
                    card.Used();
                }

                foreach (var type in recipe._result)
                    _cardFactory.GetCard(type,
                        headCard.Card.transform.position + new Vector3(0.2f, 0, 0.2f));

                onFinish?.Invoke();
            };
            return timer;
        }
    }
}