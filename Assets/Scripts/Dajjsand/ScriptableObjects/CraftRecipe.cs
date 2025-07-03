using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Dajjsand.Enums;
using UnityEngine;

namespace Dajjsand.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewCraftRecipe", menuName = "Custom/Craft Recipe")]
    public class CraftRecipe : ScriptableObject
    {
        [SerializedDictionary("CraftIngredient", "Count")]
        public SerializedDictionary<CraftIngredientType, int> _ingredients = new();
        public List<CraftIngredientType> _result;
        public float _craftTime = 10f;
        [Range(0f, 100f)]
        public float _dropChance = 100f;
    }
}