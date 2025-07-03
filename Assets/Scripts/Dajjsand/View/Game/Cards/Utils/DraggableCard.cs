using System;
using UnityEngine;

namespace Dajjsand.View.Game.Cards.Utils
{
    public class DraggableCard : MonoBehaviour
    {
        [Header("Drag Settings")]
        [SerializeField] private float _hoverHeight = 0.2f;
        [SerializeField] private LayerMask _cardLayer;
        [SerializeField] private float _mergeDistance = 0.5f;

        private Rigidbody _rigidbody;
        private Vector3 _originalPosition;
        private Quaternion _originalRotation;
        private bool _isDragging = false;
        private Camera _mainCamera;
        private DraggableCard _highlightedTarget;

        public static event Action<DraggableCard> OnHover;
        public static event Action<DraggableCard> OnHighlightForMerge;
        public static event Action<DraggableCard> OnUnhighlightForMerge;
        public static event Action<DraggableCard, DraggableCard> OnMergeCards;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _mainCamera = Camera.main;
        }

        private void OnMouseEnter()
        {
            OnHover?.Invoke(this);
        }

        private void OnMouseDown()
        {
            _originalPosition = transform.position;
            _originalRotation = transform.rotation;

            _isDragging = true;
            _rigidbody.useGravity = false;
            _rigidbody.velocity = Vector3.zero;
            transform.position += Vector3.up * _hoverHeight;
        }

        private void OnMouseDrag()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, ~0))
            {
                Vector3 targetPosition = hitInfo.point;
                targetPosition.y = _originalPosition.y + _hoverHeight;
                transform.position = targetPosition;

                CheckForMergeTarget();
            }
        }

        private void OnMouseUp()
        {
            _isDragging = false;
            _rigidbody.useGravity = true;

            if (_highlightedTarget != null)
            {
                OnMergeCards?.Invoke(this, _highlightedTarget);
                _highlightedTarget = null;
            }
            else
            {
                // Плавно возвращаемся на исходную позицию
                transform.position = _originalPosition;
                transform.rotation = _originalRotation;
            }

            OnUnhighlightForMerge?.Invoke(_highlightedTarget);
        }

        private void CheckForMergeTarget()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _mergeDistance, _cardLayer);

            DraggableCard closest = null;
            float closestDistance = float.MaxValue;

            foreach (var hit in hits)
            {
                if (hit.gameObject == this.gameObject) continue;

                var otherCard = hit.GetComponent<DraggableCard>();
                if (otherCard != null)
                {
                    float dist = Vector3.Distance(transform.position, otherCard.transform.position);
                    if (dist < closestDistance)
                    {
                        closest = otherCard;
                        closestDistance = dist;
                    }
                }
            }

            if (closest != _highlightedTarget)
            {
                if (_highlightedTarget != null)
                    OnUnhighlightForMerge?.Invoke(_highlightedTarget);

                _highlightedTarget = closest;

                if (_highlightedTarget != null)
                    OnHighlightForMerge?.Invoke(_highlightedTarget);
            }
        }
    }
}