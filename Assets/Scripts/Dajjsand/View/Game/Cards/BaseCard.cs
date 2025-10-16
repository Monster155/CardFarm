using System;
using System.Collections.Generic;
using Dajjsand.Enums;
using Dajjsand.ScriptableObjects;
using UnityEngine;

namespace Dajjsand.View.Game.Cards
{
    public class BaseCard : MonoBehaviour
    {
        public event Action<BaseCard> OnClick;
        public event Action<BaseCard> OnUsesCountEnd;
        public event Action<BaseCard, CardLogic> OnParentChanged;

        [SerializeField] private Renderer _renderer;
        [SerializeField] private DraggableCard _draggableCard;
        [SerializeField] private CardLogic _cardLogic;

        private Coroutine _mergeCoroutine;

        public int ID { get; private set; }

        public void SpawnInit(int id)
        {
            ID = id;
        }

        public void Init(CardData cardData)
        {
            _renderer.material.mainTexture = cardData._cardTexture;

            _cardLogic.Init(cardData, ID);
            _draggableCard.Init();
        }

        private void Start()
        {
            _cardLogic.OnUsesCountChanged += CardLogicOnUsesCountChanged;
            _cardLogic.OnParentChanged += CardLogic_OnParentChanged;
        }

        public void SetDraggingLockedState(bool isLocked) =>
            _draggableCard.IsDraggingLocked = isLocked;

        public void SetParentTransform(Transform parent, bool resetPosition)
        {
            transform.parent = parent;
            if (resetPosition)
                transform.localPosition = Vector3.zero;
        }

        #region PackCardLogic

        public void SetIngredients(Dictionary<CardType, int> ingredients) =>
            _cardLogic.SetCardToContainer(ingredients);

        public CardType? GetCardFromContainer(out Vector3 offset) =>
            _cardLogic.GetCardFromContainer(out offset);

        public bool IsAnyCardInContainer() =>
            _cardLogic.IsAnyCardInContainer();

        public void ReleasingCard() =>
            _cardLogic.ReleasingCard();

        #endregion

        #region Dragging

        private void OnMouseDown() => _draggableCard.TakeCard();
        private void OnMouseDrag() => _draggableCard.DragCard(); // make all other decks to check stacking and outline it if allowed
        private void OnMouseUp() => _draggableCard.PutCard();
        private void OnMouseUpAsButton() => OnClick?.Invoke(this);

        #endregion

        private void CardLogicOnUsesCountChanged(int numberOfRemainingUses)
        {
            if (numberOfRemainingUses <= 0)
                OnUsesCountEnd?.Invoke(this);
        }

        private void CardLogic_OnParentChanged(CardLogic parentCardLogic)
        {
            // someone higher should control
            OnParentChanged?.Invoke(this, parentCardLogic);
        }
    }
}