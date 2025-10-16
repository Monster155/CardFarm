using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Dajjsand.Controllers.GameLoading;
using Dajjsand.Controllers.Tasks;
using Dajjsand.Enums;
using Dajjsand.Handlers;
using Dajjsand.ScriptableObjects;
using Dajjsand.Utils.Constants;
using Dajjsand.View.Game.Cards;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Dajjsand.Factories.CardFactory
{
    public class CardFactory : ICardFactory
    {
        // events
        public event Action OnLoadComplete;

        // serialized values

        // properties
        public bool IsLoaded { get; private set; }
        private bool IsAllLoaded => _cardPrefabLoadingHandle.IsDone && _cardTexturesLoadingHandle.IsDone;

        // injected values
        private ContainersHandler _containersHandler;
        private ITasksController _tasksController;

        // private values
        private ObjectPool<BaseCard> _cardPool;
        private AsyncOperationHandle<GameObject> _cardPrefabLoadingHandle;
        private AsyncOperationHandle<IList<CardData>> _cardTexturesLoadingHandle;
        private BaseCard _baseCardPrefab;

        private SerializedDictionary<CardType, CardData> _cards;


        public CardFactory(ILoadController loadController, ContainersHandler containersHandler, ITasksController tasksController)
        {
            loadController.AddLoadable(this);

            _containersHandler = containersHandler;
            _tasksController = tasksController;

            _cardPool = new ObjectPool<BaseCard>(CreateCard);

            _cardPrefabLoadingHandle = Addressables.LoadAssetAsync<GameObject>(AddressablePathConstants.BaseCardPrefab);
            _cardPrefabLoadingHandle.Completed += CardPrefabLoadingHandle_Completed;

            _cardTexturesLoadingHandle = Addressables.LoadAssetsAsync<CardData>(AddressablePathConstants.CardsData);
            _cardTexturesLoadingHandle.Completed += CardTexturesLoadingHandle_Completed;
        }

        ~CardFactory()
        {
            _cardPool.Dispose();
            _cardPrefabLoadingHandle.Completed -= CardPrefabLoadingHandle_Completed;
            _cardTexturesLoadingHandle.Completed -= CardTexturesLoadingHandle_Completed;
        }

        public BaseCard GetCard(CardType cardType, Vector3 pos)
        {
            var card = _cardPool.Get();
            card.gameObject.SetActive(true);

            card.Init(_cards[cardType]);
            card.name = cardType.ToString();
            card.transform.position = pos;

            _tasksController.UpdateReceivedCards(cardType);

            return card;
        }

        public bool ReleaseCard(BaseCard card)
        {
            card.ReleasingCard();
            card.transform.position = new Vector3(0f, -100f, 0f);
            card.transform.parent = _containersHandler.CardsContainer;
            card.gameObject.SetActive(false);
            _cardPool.Release(card);
            return true;
        }

        public List<BaseCard> GetPacks(List<CardPackData> packs) =>
            packs.Select(GetPack).ToList();

        public BaseCard GetPack(CardPackData packData)
        {
            var pack = GetCard(CardType.Pack, new Vector3(0f, 0.2f, 0f));
            pack.SetIngredients(new Dictionary<CardType, int>(packData._cardsInside));
            return pack;
        }

        private BaseCard CreateCard()
        {
            if (!IsLoaded)
            {
                Debug.LogError("Card Prefab doesn't loaded yet");
                return null;
            }

            var card = Object.Instantiate(_baseCardPrefab, _containersHandler.CardsContainer);
            card.SpawnInit(_cardPool.CountAll);
            card.OnUsesCountEnd += Card_OnUsesCountEnd;
            card.OnParentChanged += Card_OnParentChanged;
            return card;
        }

        private void Card_OnUsesCountEnd(BaseCard baseCard) =>
            ReleaseCard(baseCard);

        private void Card_OnParentChanged(BaseCard baseCard, CardLogic parentCardLogic)
        {
            if (parentCardLogic != null)
                baseCard.SetParentTransform(parentCardLogic.ChildContainer, true);
            else
                baseCard.SetParentTransform(_containersHandler.CardsContainer, false);
        }

        private void UpdateLoadingState()
        {
            if (IsAllLoaded)
            {
                IsLoaded = true;
                OnLoadComplete?.Invoke();
            }
        }


        private void CardPrefabLoadingHandle_Completed(AsyncOperationHandle<GameObject> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _baseCardPrefab = handle.Result.GetComponent<BaseCard>();
            }
            else Debug.LogError($"{nameof(CardPrefabLoadingHandle_Completed)} finished with {handle.Status} status");

            UpdateLoadingState();
        }

        private void CardTexturesLoadingHandle_Completed(AsyncOperationHandle<IList<CardData>> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var cards = handle.Result;

                _cards = new();
                foreach (CardData card in cards)
                    _cards.Add(card._cardType, card);
            }
            else Debug.LogError($"{nameof(CardTexturesLoadingHandle_Completed)} finished with {handle.Status} status");

            UpdateLoadingState();
        }
    }
}