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
        private AsyncOperationHandle<IngredientsTextures> _cardTexturesLoadingHandle;
        private BaseCard _baseCardPrefab;

        private ContainersHandler _containersHandler;

        private bool _isCardPrefabsLoaded;
        private bool _isTexturesLoaded;

        private SerializedDictionary<CraftIngredientType, Texture> _ingredientToTexture;

        public CardFactory(ContainersHandler containersHandler)
        {
            _containersHandler = containersHandler;

            _cardPool = new ObjectPool<BaseCard>(CreateCard);

            _cardPrefabLoadingHandle = Addressables.LoadAssetAsync<GameObject>(AddressablePathConstants.BaseCardPrefab);
            _cardPrefabLoadingHandle.Completed += OnCardLoadingComplete;

            _cardTexturesLoadingHandle = Addressables.LoadAssetAsync<IngredientsTextures>(AddressablePathConstants.CardTextures);
            _cardTexturesLoadingHandle.Completed += OnTexturesLoadingComplete;
        }

        ~CardFactory()
        {
            _cardPool.Dispose();
            _cardPrefabLoadingHandle.Completed -= OnCardLoadingComplete;
            _cardTexturesLoadingHandle.Completed -= OnTexturesLoadingComplete;
        }

        public BaseCard GetCard(CraftIngredientType ingredientType)
        {
            var card = _cardPool.Get();
            card.Init(ingredientType, _ingredientToTexture[ingredientType]);
            return card;
        }

        public bool ReleaseCard(BaseCard card)
        {
            _cardPool.Release(card);
            return true;
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

        private void OnTexturesLoadingComplete(AsyncOperationHandle<IngredientsTextures> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _ingredientToTexture = handle.Result._ingredientToTexture;

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