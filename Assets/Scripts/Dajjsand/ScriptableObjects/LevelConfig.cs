using System.Collections.Generic;
using Dajjsand.Enums;
using UnityEngine;

namespace Dajjsand.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelConfig 1", menuName = "Custom/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        public int _levelNumber;
        public List<CraftRecipe> _availableRecipes = new();
        public List<CraftIngredientType> _startIngredients = new();
    }
}