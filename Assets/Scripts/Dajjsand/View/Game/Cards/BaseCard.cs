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
        public event Action<BaseCard> OnDestroy;

        [SerializeField] private Renderer _renderer;
        [SerializeField] private MergeBar _mergeBar;
        [SerializeField] private Transform _childContainer;
        [SerializeField] private DraggableCard _draggableCard;

        private Transform _cardsContainer;
        private CardLogic _cardLogic = new CardLogic();
        private Coroutine _mergeCoroutine;

        public void Init(CardData cardData, Transform cardsContainer)
        {
            _cardsContainer = cardsContainer;
            _cardLogic.Init(cardData, this);

            _renderer.material.mainTexture = cardData._cardTexture;

            _draggableCard.Init(_cardLogic);
        }

        private void Start()
        {
            _cardLogic.OnUsesCountEnd += () => OnDestroy?.Invoke(this);
            _cardLogic.OnParentChanged += CardLogic_OnParentChanged;
            _cardLogic.OnMergeTimerUpdate += CardLogic_OnMergeTimerUpdate;
            _cardLogic.OnMergeTimerStart += CardLogic_OnMergeTimerStart;
            _cardLogic.OnMergeTimerStop += CardLogic_OnMergeTimerStop;
            _cardLogic.OnMergeTimerFinish += CardLogic_OnMergeTimerFinish;
        }

        public void SetIngredients(Dictionary<CardType, int> ingredients) =>
            _cardLogic.SetCardToContainer(ingredients);

        public CardType? GetCardFromContainer() =>
            _cardLogic.GetCardFromContainer();

        public bool IsAnyCardInContainer() =>
            _cardLogic.IsAnyCardInContainer();

        #region Dragging

        private void OnMouseDown() => _draggableCard.MouseDown();
        private void OnMouseDrag() => _draggableCard.MouseDrag();
        private void OnMouseUp() => _draggableCard.MouseUp();

        #endregion

        private void CardLogic_OnParentChanged(BaseCard parentCard)
        {
            if (parentCard == null)
            {
                transform.parent = _cardsContainer;
            }
            else
            {
                transform.parent = parentCard._childContainer;
                transform.localPosition = Vector3.zero;
            }
        }

        private void CardLogic_OnMergeTimerUpdate(float percentage) => _mergeBar.UpdateProgress(percentage);
        private void CardLogic_OnMergeTimerStart() => _mergeBar.StartMerge();
        private void CardLogic_OnMergeTimerStop() => _mergeBar.StopMerge();
        private void CardLogic_OnMergeTimerFinish() => _mergeBar.FinishMerge();

        private void OnMouseUpAsButton() => OnClick?.Invoke(this);

        public void SetDraggingLockedState(bool isLocked) => _draggableCard.IsDraggingLocked = isLocked;
    }
}