using AYellowpaper.SerializedCollections;
using Dajjsand.Enums;
using UnityEngine;

namespace Dajjsand.ScriptableObjects
{
    [CreateAssetMenu(fileName = "IngredientsTextures", menuName = "Custom/IngredientsTextures", order = 0)]
    public class IngredientsTextures : ScriptableObject
    {
        [SerializedDictionary("Ingredient", "Texture")]
        public SerializedDictionary<CardType, Texture> _ingredientToTexture = new();
    }
}