using System.Collections.Generic;
using Dajjsand.DataAndModel.Receipts;
using UnityEngine;

namespace Dajjsand.DataAndModel
{
    [CreateAssetMenu(fileName = "LevelConfig 1", menuName = "Custom/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        public string _levelName;
        public List<CraftRecipe> _availableRecipes = new();
    }
}