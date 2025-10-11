using AYellowpaper.SerializedCollections;
using Dajjsand.Enums;
using UnityEngine;

namespace Dajjsand.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewStoreConfig", menuName = "Custom/Store Config", order = 0)]
    public class StoreConfig : ScriptableObject
    {
        public Texture _cardTexture;
        [SerializedDictionary("Card", "Count")]
        public SerializedDictionary<CardType, int> _price;
        [SerializedDictionary("Card Pack", "Random Weight")]
        public SerializedDictionary<CardPackData, float> _dropdownPacks;
    }
}