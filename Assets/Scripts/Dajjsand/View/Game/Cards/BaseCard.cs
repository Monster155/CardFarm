using System;
using System.Collections;
using System.Collections.Generic;
using Dajjsand.Controllers.Craft;
using Dajjsand.Enums;
using Dajjsand.ScriptableObjects;
using ModestTree;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Dajjsand.View.Game.Cards
{
    public class BaseCard : MonoBehaviour
    {
        public event Action<BaseCard> OnClick;

        [SerializeField] private Renderer _renderer;
        [SerializeField] private MergeBar _mergeBar;
        [SerializeField] private Transform _childContainer;
        [SerializeField] private DraggableCard _draggableCard;

        private Transform _cardsContainer;
        private CardLogic _cardLogic;
        private Coroutine _mergeCoroutine;

        public void Init(CardData cardData, Transform cardsContainer)
        {
            _cardsContainer = cardsContainer;
            _cardLogic = new CardLogic(cardData, this);

            _renderer.material.mainTexture = cardData._cardTexture;

            _draggableCard.Init(_cardLogic);

            _cardLogic.OnUsesCountEnd += () => Destroy(gameObject);
            _cardLogic.OnParentChanged += CardLogic_OnParentChanged;
        }

        public void Used() =>
            _cardLogic.Used();

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

        public void StopMergeTimer()
        {
            if (_mergeCoroutine != null)
                StopCoroutine(_mergeCoroutine);
            _mergeBar.StopMerge();
        }

        private IEnumerator MergeTimer(Action<float> callback, float duration)
        {
            _mergeBar.StartMerge();
            float timer = 0f;
            while (timer < duration)
            {
                timer -= Time.deltaTime;
                yield return null;

                callback?.Invoke(timer / duration);
                _mergeBar.UpdateProgress(timer / duration);
            }

            _mergeBar.FinishMerge();
        }

        private void OnMouseUpAsButton() => OnClick?.Invoke(this);

        public void SetDraggingLockedState(bool isLocked)
        {
        }
    }
}