using Dajjsand.Enums;
using UnityEngine;

namespace Dajjsand.View.Game.Cards
{
    public class BaseCard : MonoBehaviour
    {
        private CraftIngredient _ingredient;

        public void Init(CraftIngredient ingredient)
        {
            _ingredient = ingredient;
        }
    }
}