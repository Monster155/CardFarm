using AYellowpaper.SerializedCollections;
using Dajjsand.Enums;
using UnityEngine;

namespace Dajjsand.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewCardPackData", menuName = "Custom/Card Pack Data", order = 0)]
    public class CardPackData : ScriptableObject
    {
        [SerializedDictionary("Card", "Count")]
        public SerializedDictionary<CardType, int> _cardsInside;
    }
}