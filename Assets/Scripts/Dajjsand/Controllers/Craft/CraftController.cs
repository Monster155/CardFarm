using System.Collections.Generic;
using System.Linq;
using Dajjsand.Enums;
using Dajjsand.ScriptableObjects;
using Dajjsand.View.Game.Cards;
using UnityEngine;

namespace Dajjsand.Controllers.Craft
{
    public class CraftController
    {
        public static CraftController Instance { get; private set; }

        private Dictionary<CardType, List<CraftRecipe>> _recipeDictionary;

        public CraftController(List<CraftRecipe> availableRecipes)
        {
            if (Instance != null)
            {
                Debug.LogError("Use CraftController.Instance instead of creating a new one");
                return;
            }

            Instance = this;

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

        // public List<CraftRecipe> GetRecipesForStock(BaseCard headCard)
        // {
        //     if (!_recipeDictionary.TryGetValue(headCard.Type, out var recipes))
        //         return null;
        //
        //     HashSet<CraftRecipe> set = new HashSet<CraftRecipe>(recipes);
        //
        //     var card = headCard;
        //     while (card.Child != null)
        //     {
        //         card = card.Child;
        //         if (!_recipeDictionary.TryGetValue(card.Type, out var recipesChild))
        //             return null;
        //
        //         set.RemoveWhere(recipe => !recipesChild.Contains(recipe));
        //     }
        //
        //     return set.ToList();
        // }
        //
        // public bool TryToStartMerge(Dictionary<CardType, int> ingredients)
        // {
        //     var recipes = GetRecipesForStock(headCard);
        //     
        //     if(recipes == null || recipes.Count == 0)
        //         return false;
        //
        //     headCard.StartMergeTimer(TimerCallback, recipes[0]._craftTime);
        //     
        //     return true;
        // }
        //
        // public void StopMerge(BaseCard headCard)
        // {
        //     headCard.StopMergeTimer();
        // }
        //
        // private void TimerCallback(float percentage){}
    }
}