using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Dajjsand.DataAndModel.Receipts
{
    [CreateAssetMenu(fileName = "NewCraftRecipe", menuName = "CardFarm/Craft Recipe")]
    public class CraftRecipe : ScriptableObject
    {
        [SerializedDictionary("CraftIngredient", "Count")]
        public SerializedDictionary<CraftIngredient, int> _ingredients = new();
        public string _result;
        public float _craftTime = 10f;
        [Range(0f, 100f)]
        public float _dropChance = 100f;
    }
}