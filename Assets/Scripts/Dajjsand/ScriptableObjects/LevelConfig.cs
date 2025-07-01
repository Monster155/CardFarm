using System.Collections.Generic;
using UnityEngine;

namespace Dajjsand.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelConfig 1", menuName = "Custom/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        public string _levelName;
        public List<CraftRecipe> _availableRecipes = new();
    }
}