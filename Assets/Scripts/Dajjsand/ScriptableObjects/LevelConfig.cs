using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Dajjsand.Models.Task;
using UnityEngine;

namespace Dajjsand.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelConfig 1", menuName = "Custom/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        public int _levelNumber;
        [Space]
        public List<CraftRecipe> _availableRecipes = new();
        [Space]
        [SerializedDictionary("CraftIngredient", "Count")]
        public List<CardPackData> _starterPacks;
        [Space]
        public Tasks _tasksToCompleteLevel;
        [Header("Store")]
        public bool _hasSellStore = true;
        [Space]
        public List<StoreConfig> _storeConfigs;
    }
}