using Dajjsand.Enums;
using UnityEngine;

namespace Dajjsand.View.Game.Cards
{
    public class BaseCard : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private int _numberOfRemainingUses = 1;

        private CraftIngredientType _ingredientType;

        public void Init(CraftIngredientType ingredientType, Texture texture)
        {
            _ingredientType = ingredientType;
            _renderer.material.mainTexture = texture;
        }

        public virtual void Used()
        {
            _numberOfRemainingUses--;
            if (_numberOfRemainingUses <= 0)
                Destroy(gameObject);
        }
    }
}