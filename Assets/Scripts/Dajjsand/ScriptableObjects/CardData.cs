using Dajjsand.Enums;
using UnityEngine;

namespace Dajjsand.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewCardData", menuName = "Custom/Card Data", order = 0)]
    public class CardData : ScriptableObject
    {
        public CardType _cardType;
        public Texture _cardTexture;
        public int _numberOfUses;
    }
}