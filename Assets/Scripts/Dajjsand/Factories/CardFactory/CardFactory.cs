using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Dajjsand.Controllers.Loading;
using Dajjsand.Enums;
using Dajjsand.Handlers;
using Dajjsand.ScriptableObjects;
using Dajjsand.Utils;
using Dajjsand.Utils.Constants;
using Dajjsand.View.Game.Cards;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Dajjsand.Factories.CardFactory
{
    public class CardFactory : ICardFactory, ILoadable
    {
        public event Action OnLoadComplete;

        public bool IsLoaded { get; private set; }

        private ObjectPool<BaseCard> _cardPool;
        private AsyncOperationHandle<GameObject> _cardPrefabLoadingHandle;
        private AsyncOperationHandle<IList<CardData>> _cardTexturesLoadingHandle;
        private BaseCard _baseCardPrefab;

        private ContainersHandler _containersHandler;

        private bool _isCardPrefabsLoaded;
        private bool _isTexturesLoaded;

        private SerializedDictionary<CardType, CardData> _cards;

        public CardFactory(ContainersHandler containersHandler)
        {
            _containersHandler = containersHandler;

            _cardPool = new ObjectPool<BaseCard>(CreateCard);

            _cardPrefabLoadingHandle = Addressables.LoadAssetAsync<GameObject>(AddressablePathConstants.BaseCardPrefab);
            _cardPrefabLoadingHandle.Completed += OnCardLoadingComplete;

            _cardTexturesLoadingHandle = Addressables.LoadAssetsAsync<CardData>(AddressablePathConstants.CardsData);
            _cardTexturesLoadingHandle.Completed += OnTexturesLoadingComplete;
        }

        ~CardFactory()
        {
            _cardPool.Dispose();
            _cardPrefabLoadingHandle.Completed -= OnCardLoadingComplete;
            _cardTexturesLoadingHandle.Completed -= OnTexturesLoadingComplete;
        }

        public BaseCard GetCard(CardType cardType, Vector3 pos)
        {
            var card = _cardPool.Get();
            card.gameObject.SetActive(true);
            
            card.Init(_cards[cardType]);
            card.name = cardType.ToString();
            card.transform.position = pos;
            
            return card;
        }

        public bool ReleaseCard(BaseCard card)
        {
            card.transform.position = new Vector3(0f, -100f, 0f);
            card.gameObject.SetActive(false);
            _cardPool.Release(card);
            return true;
        }

        public BaseCard GetStarterPack(Dictionary<CardType, int> ingredients)
        {
            var pack = GetCard(CardType.Pack, new Vector3(0f, 0.2f, 0f));
            pack.SetIngredients(ingredients);
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
            return card;
        }

        private void OnCardLoadingComplete(AsyncOperationHandle<GameObject> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _baseCardPrefab = handle.Result.GetComponent<BaseCard>();

                _isCardPrefabsLoaded = true;
                if (_isTexturesLoaded && _isCardPrefabsLoaded)
                {
                    IsLoaded = true;
                    OnLoadComplete?.Invoke();
                }
            }
        }

        private void OnTexturesLoadingComplete(AsyncOperationHandle<IList<CardData>> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var cards = handle.Result;

                _cards = new();
                foreach (CardData card in cards)
                    _cards.Add(card._cardType, card);

                _isTexturesLoaded = true;
                if (_isTexturesLoaded && _isCardPrefabsLoaded)
                {
                    IsLoaded = true;
                    OnLoadComplete?.Invoke();
                }
            }
        }
    }
}