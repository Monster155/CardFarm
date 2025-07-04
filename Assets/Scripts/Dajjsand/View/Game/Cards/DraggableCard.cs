using Dajjsand.Controllers.Craft;
using Dajjsand.Enums;
using UnityEngine;

namespace Dajjsand.View.Game.Cards
{
    public class DraggableCard : MonoBehaviour
    {
        [SerializeField] private float _hoverHeight = 0.2f;
        [SerializeField] private LayerMask _cardLayer;
        [SerializeField] private LayerMask _mapLayer;
        [Space]
        [SerializeField] private float _pushAmount = 1f;
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }

        public bool IsDraggingLocked { get; set; } = false;
        public bool IsDragging { get; private set; } = false;
        public CardLogic Logic { get; private set; }

        private Camera _mainCamera;
        public float LastDragTime { get; private set; } = 0f;

        public void Init(CardLogic cardLogic)
        {
            Logic = cardLogic;
            Logic.OnParentChanged += BaseCard_OnParentChanged;

            _mainCamera = Camera.main;
        }

        #region Dragging

        public void MouseDown()
        {
            if (IsDraggingLocked)
                return;

            Logic.LoseChildren();
            IsDragging = true;
            Rigidbody.useGravity = false;
            Rigidbody.linearVelocity = Vector3.zero;
            Rigidbody.transform.position += Vector3.up * _hoverHeight;
            Rigidbody.transform.rotation = Quaternion.identity;
        }

        public void MouseDrag()
        {
            if (IsDraggingLocked)
                return;

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, _mapLayer))
            {
                Vector3 targetPosition = hitInfo.point;
                targetPosition.y += _hoverHeight;
                Rigidbody.transform.position = targetPosition;
            }
        }

        public void MouseUp()
        {
            // if locked on move
            IsDragging = false;
            Rigidbody.useGravity = true;

            LastDragTime = Time.time;

            if (IsDraggingLocked)
                return;

            // do smth
        }

        #endregion

        private void OnTriggerStay(Collider other)
        {
            var otherCollider = other.GetComponent<DraggableCard>();

            if (IsDragging || otherCollider.IsDragging)
                return;

            if (Logic.HeadCard.Equals(otherCollider.Logic.HeadCard))
                return;

            Vector3 delta = transform.position - other.transform.position;
            delta.y = 0;

            if (CanCardsBeMerged(Logic.Type, otherCollider.Logic.Type))
            {
                if (LastDragTime < otherCollider.LastDragTime)
                    Logic.MakeParenting(otherCollider.Logic);
            }
            else
            {
                Vector3 pushDir = delta.normalized;
                Rigidbody.AddForce(pushDir * _pushAmount, ForceMode.Force);
                otherCollider.Rigidbody.AddForce(-pushDir * _pushAmount, ForceMode.Force);
            }
        }

        private void BaseCard_OnParentChanged(BaseCard parentCard)
        {
            Rigidbody.isKinematic = parentCard != null;
        }

        private bool CanCardsBeMerged(CardType card1, CardType card2) =>
            CraftController.Instance.CanBeMerged(card1, card2);
    }
}