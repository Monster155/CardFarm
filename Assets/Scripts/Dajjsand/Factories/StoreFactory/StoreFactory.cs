using System;
using System.Collections.Generic;
using System.Linq;
using Dajjsand.Controllers.GameLoading;
using Dajjsand.Handlers;
using Dajjsand.ScriptableObjects;
using Dajjsand.Utils.Constants;
using Dajjsand.View.Game.Store;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Dajjsand.Factories.StoreFactory
{
    public class StoreFactory : IStoreFactory
    {
        // events
        public event Action OnLoadComplete;

        // serialized values

        // properties
        public bool IsLoaded { get; private set; }
        private bool IsAllLoaded => _sellStoreLoadingHandle.IsDone && _storeCardPrefabLoadingHandle.IsDone;

        // injected values
        private ContainersHandler _containersHandler;

        // private values
        private AsyncOperationHandle<SellStoreConfig> _sellStoreLoadingHandle;
        private SellStoreConfig _sellStoreConfig;
        private AsyncOperationHandle<GameObject> _storeCardPrefabLoadingHandle;
        private StoreCard _storeCardPrefab;

        public StoreFactory(ILoadController loadController, ContainersHandler containersHandler)
        {
            loadController.AddLoadable(this);

            _containersHandler = containersHandler;

            _sellStoreLoadingHandle = Addressables.LoadAssetAsync<SellStoreConfig>(AddressablePathConstants.SellStoreConfig);
            _sellStoreLoadingHandle.Completed += SellStoreLoadingHandle_Completed;

            _storeCardPrefabLoadingHandle = Addressables.LoadAssetAsync<GameObject>(AddressablePathConstants.StoreCardPrefab);
            _storeCardPrefabLoadingHandle.Completed += StoreCardPrefabLoadingHandle_Completed;
        }

        ~StoreFactory()
        {
            _sellStoreLoadingHandle.Completed -= SellStoreLoadingHandle_Completed;
            _storeCardPrefabLoadingHandle.Completed -= StoreCardPrefabLoadingHandle_Completed;
        }

        public void SpawnSellStore()
        {
            var storeCard = Object.Instantiate(_storeCardPrefab, _containersHandler.StoreCardsContainer);
            storeCard.InitSellStore(_sellStoreConfig);
        }

        public void SpawnStore(StoreConfig storeConfig)
        {
            var storeCard = Object.Instantiate(_storeCardPrefab, _containersHandler.StoreCardsContainer);
            storeCard.InitDefaultStore(storeConfig);
        }

        public void SpawnStores(List<StoreConfig> storeConfigs)
        {
            foreach (StoreConfig storeConfig in storeConfigs)
                SpawnStore(storeConfig);
        }

        private void UpdateLoadingState()
        {
            if (IsAllLoaded)
            {
                IsLoaded = true;
                OnLoadComplete?.Invoke();
            }
        }

        private void SellStoreLoadingHandle_Completed(AsyncOperationHandle<SellStoreConfig> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _sellStoreConfig = handle.Result;
            }
            else Debug.LogError($"{nameof(SellStoreLoadingHandle_Completed)} finished with {handle.Status} status");

            UpdateLoadingState();
        }

        private void StoreCardPrefabLoadingHandle_Completed(AsyncOperationHandle<GameObject> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _storeCardPrefab = handle.Result.GetComponent<StoreCard>();
            }
            else Debug.LogError($"{nameof(StoreCardPrefabLoadingHandle_Completed)} finished with {handle.Status} status");

            UpdateLoadingState();
        }
    }
}