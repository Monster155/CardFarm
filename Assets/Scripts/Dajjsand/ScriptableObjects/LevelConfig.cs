using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Dajjsand.Enums;
using Dajjsand.Models.Task;
using UnityEngine;

namespace Dajjsand.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelConfig 1", menuName = "Custom/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        public int _levelNumber;
        public List<CraftRecipe> _availableRecipes = new();
        [SerializedDictionary("CraftIngredient", "Count")]
        public SerializedDictionary<CardType, int> _startIngredients = new();
        public Tasks _tasksToCompleteLevel;
    }
}