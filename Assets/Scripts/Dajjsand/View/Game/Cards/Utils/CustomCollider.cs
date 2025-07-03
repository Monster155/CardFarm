using Dajjsand.Controllers.Craft;
using UnityEngine;

namespace Dajjsand.View.Game.Cards.Utils
{
    public class CustomCollider : MonoBehaviour
    {
        [SerializeField] private float _pushAmount = 1f;
        [SerializeField] private DraggableCard _draggableCard;
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public BaseCard Card { get; private set; }

        private void OnTriggerStay(Collider other)
        {
            if (_draggableCard.IsDragging)
                return;

            var otherCollider = other.GetComponent<CustomCollider>();

            Vector3 delta = transform.position - other.transform.position;
            delta.y = 0;

            if (CraftController.Instance.CanBeMerged(Card.IngredientType, otherCollider.Card.IngredientType))
            {
                
            }
            else
            {
                Vector3 pushDir = delta.normalized;
                Rigidbody.AddForce(pushDir * _pushAmount, ForceMode.Force);
                otherCollider.Rigidbody.AddForce(-pushDir * _pushAmount, ForceMode.Force);
            }
        }
    }
}