using System;
using Dajjsand.Controllers.Craft;
using Dajjsand.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dajjsand.View.Game.Cards
{
    public class DraggableCard : MonoBehaviour
    {
        [SerializeField] private float _hoverHeight = 0.2f;
        [SerializeField] private LayerMask _cardLayer;
        [SerializeField] private LayerMask _mapLayer;
        [SerializeField] private CardLogic _cardLogic;
        [Space]
        [SerializeField] private float _pushAmount = 7f;
        [SerializeField] private Rigidbody _rigidbody;

        public bool IsDraggingLocked
        {
            get => _isDraggingLocked;
            set
            {
                _isDraggingLocked = value;
                if (value)
                    PutCard();
            }
        }

        public bool IsDragging { get; private set; } = false;
        public float LastDragTime { get; private set; } = 0f;

        private Camera _mainCamera;
        private bool _isDraggingLocked;


        public void Init()
        {
            _mainCamera = Camera.main;
            IsDraggingLocked = false;
            LastDragTime = Time.time;
        }

        private void Start()
        {
            _cardLogic.OnParentChanged += CardLogic_OnParentChanged;
        }

        #region Dragging

        public void TakeCard()
        {
            if (IsDraggingLocked)
                return;

            _cardLogic.LoseParentDeck(); // should always be before isKinematic change

            IsDragging = true;

            _rigidbody.isKinematic = true;
            _rigidbody.transform.position += Vector3.up * _hoverHeight;
        }

        public void DragCard()
        {
            if (IsDraggingLocked)
                return;

            LastDragTime = Time.time;

            MoveCardToMouse();
        }

        public void PutCard()
        {
            // if locked on move
            _rigidbody.isKinematic = false;
            IsDragging = false;

            LastDragTime = Time.time;

            if (IsDraggingLocked)
                return;

            // do smth
        }

        #endregion

        private void MoveCardToMouse()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, _mapLayer))
            {
                Vector3 targetPosition = hitInfo.point;
                targetPosition += Vector3.up * _hoverHeight;
                _rigidbody.transform.position = targetPosition;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            var otherCard = other.GetComponent<DraggableCard>();

            if (IsInSameDeck(this, otherCard))
            {
                // disable all cards triggers and left only for HeadCard
                Debug.LogWarning("You trying to merge cards in same deck");
                return;
            }

            // ignore it over card moves then dragging
            if (otherCard.IsDragging)
                return;

            if (IsDragging) // make all other decks to check stacking and outline it if allowed
                return;

            if (_cardLogic.IsMaxSizeDeck(otherCard._cardLogic.DeckSize))
                return;

            if (CanCardsBeStacked(_cardLogic.Type, otherCard._cardLogic.Type))
            {
                if (LastDragTime < otherCard.LastDragTime)
                    _cardLogic.AddCardsFrom(otherCard._cardLogic);
            }
            else
                PushAwayCards(this, otherCard);
        }

        private void CardLogic_OnParentChanged(CardLogic cardLogic)
        {
            _rigidbody.isKinematic = cardLogic != null;
        }

        private bool CanCardsBeStacked(CardType card1, CardType card2) =>
            CraftController.Instance?.CanBeMerged(card1, card2) ?? false;

        private void PushAwayCards(DraggableCard card1, DraggableCard card2)
        {
            Vector3 delta = card1.transform.position - card2.transform.position;
            delta.y = 0;
            Vector3 pushDir = delta.normalized;

            if (pushDir.magnitude < 0.001f)
                pushDir = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));

            card1._rigidbody.AddForce(pushDir * _pushAmount, ForceMode.Force);
            card2._rigidbody.AddForce(-pushDir * _pushAmount, ForceMode.Force);
        }

        private bool IsInSameDeck(DraggableCard card1, DraggableCard card2) =>
            card1._cardLogic.HeadCard.ID == card2._cardLogic.HeadCard.ID;
    }
}