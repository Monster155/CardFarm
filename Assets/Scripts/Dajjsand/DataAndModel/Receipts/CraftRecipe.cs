using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Dajjsand.DataAndModel.Receipts
{
    [CreateAssetMenu(fileName = "NewCraftRecipe", menuName = "Custom/Craft Recipe")]
    public class CraftRecipe : ScriptableObject
    {
        [SerializedDictionary("CraftIngredient", "Count, Doesn't disappear")]
        public SerializedDictionary<CraftIngredient, IngredientData> _ingredients = new();
        public List<CraftIngredient> _result;
        public float _craftTime = 10f;
        [Range(0f, 100f)]
        public float _dropChance = 100f;
        
        [Serializable]
        public class IngredientData
        {
            public int Count;
            public bool NotDisappears;
        }
    }
}