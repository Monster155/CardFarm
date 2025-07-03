using UnityEngine;
using System;
using System.Collections;

namespace Dajjsand.View.Game.Cards.Utils
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class DraggableCard : MonoBehaviour
    {
        [Header("Drag Settings")]
        [SerializeField] private float _hoverHeight = 0.2f;
        [SerializeField] private LayerMask _cardLayer;
        [SerializeField] private LayerMask _mapLayer;
        [SerializeField] private float _mergeDistance = 0.5f;
        [SerializeField] private float _repelForce = 3f;
        [SerializeField] private float _repelDuration = 0.2f;
        [SerializeField] private float _repelBoxPadding = 0.05f;
        [Header("Links")]
        [SerializeField] private BaseCard _baseCard;

        public bool IsDragging { get; private set; } = false;

        private Rigidbody _rigidbody;
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
            if(_baseCard.IsDraggingLocked)
                return;
            
            IsDragging = true;
            _rigidbody.useGravity = false;
            _rigidbody.linearVelocity = Vector3.zero;
            transform.position += Vector3.up * _hoverHeight;
            transform.rotation = Quaternion.identity;
        }

        private void OnMouseDrag()
        {
            if(_baseCard.IsDraggingLocked)
                return;
            
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, _mapLayer))
            {
                Vector3 targetPosition = hitInfo.point;
                targetPosition.y += _hoverHeight;
                transform.position = targetPosition;

                CheckForMergeTarget();
            }
        }

        private void OnMouseUp()
        {
            // if locked on move
            IsDragging = false;
            _rigidbody.useGravity = true;
            
            if(_baseCard.IsDraggingLocked)
                return;

            if (_highlightedTarget != null)
            {
                OnMergeCards?.Invoke(this, _highlightedTarget);
                _highlightedTarget = null;
            }
            else
            {
                // Оставляем карточку на месте
                StartCoroutine(RepelFromOverlappingCards());
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

        private IEnumerator RepelFromOverlappingCards()
        {
            BoxCollider myCollider = GetComponent<BoxCollider>();
            Vector3 halfExtents = myCollider.size * 0.5f * transform.localScale.x + Vector3.one * _repelBoxPadding;

            Collider[] overlaps = Physics.OverlapBox(transform.position, halfExtents, transform.rotation, _cardLayer);

            foreach (var col in overlaps)
            {
                if (col.gameObject == this.gameObject) continue;

                Vector3 direction = (transform.position - col.transform.position).normalized;
                direction.y = 0;

                if (direction == Vector3.zero)
                    direction = new Vector3(UnityEngine.Random.value, 0, UnityEngine.Random.value).normalized;

                _rigidbody.AddForce(direction * _repelForce, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(_repelDuration);

            _rigidbody.linearVelocity = Vector3.zero;
        }

#if UNITY_EDITOR
        // Для наглядности в редакторе
        private void OnDrawGizmosSelected()
        {
            if (!TryGetComponent(out BoxCollider col)) return;

            Gizmos.color = Color.cyan;
            Vector3 halfExtents = col.size * 0.5f * transform.localScale.x + Vector3.one * _repelBoxPadding;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2);
        }
#endif
    }
}