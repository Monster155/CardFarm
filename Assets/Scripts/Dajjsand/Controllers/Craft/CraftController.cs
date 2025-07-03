using System.Collections.Generic;
using Dajjsand.Enums;
using Dajjsand.ScriptableObjects;
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

        // public bool TryCraft(List<string> inputResources, out string result)
        // {
        //     foreach (var recipe in _currentLevelRecipes.availableRecipes)
        //     {
        //         if (RecipeMatches(recipe, inputResources))
        //         {
        //             result = recipe.result;
        //             return true;
        //         }
        //     }
        //
        //     result = null;
        //     return false;
        // }
        //
        // private bool RecipeMatches(CraftRecipe recipe, List<string> inputResources)
        // {
        //     var required = recipe.ingredients
        //         .GroupBy(i => i.resourceName)
        //         .ToDictionary(g => g.Key, g => g.Sum(i => i.amount));
        //
        //     var inputGrouped = inputResources
        //         .GroupBy(r => r)
        //         .ToDictionary(g => g.Key, g => g.Count());
        //
        //     foreach (var req in required)
        //     {
        //         if (!inputGrouped.TryGetValue(req.Key, out var inputAmount) || inputAmount < req.Value)
        //             return false;
        //     }
        //
        //     return true;
        // }
    }
}