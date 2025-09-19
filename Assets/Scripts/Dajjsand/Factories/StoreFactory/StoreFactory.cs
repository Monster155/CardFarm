using System;
using System.Collections.Generic;
using Dajjsand.Handlers;
using Dajjsand.ScriptableObjects;
using Dajjsand.Utils.Constants;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Dajjsand.Factories.StoreFactory
{
    public class StoreFactory : IStoreFactory
    {
        // events
        public event Action OnLoadComplete;

        // serialized values

        // properties
        public bool IsLoaded { get; private set; }

        // injected values
        private ContainersHandler _containersHandler;

        // private values
        private AsyncOperationHandle<IList<StoreConfig>> _sellStoreLoadingHandle;

        public StoreFactory(ContainersHandler containersHandler)
        {
            _containersHandler = containersHandler;

            _sellStoreLoadingHandle = Addressables.LoadAssetsAsync<StoreConfig>(AddressablePathConstants.SellStore);
            _sellStoreLoadingHandle.Completed += OnSellStoreLoadingComplete;
        }

        ~StoreFactory()
        {
            _sellStoreLoadingHandle.Completed -= OnSellStoreLoadingComplete;
        }

        public void SpawnSellStore()
        {
        }

        public void SpawnStores(List<StoreConfig> storeConfigs)
        {
            foreach (StoreConfig storeConfig in storeConfigs)
                SpawnStore(storeConfig);
        }

        public void SpawnStore(StoreConfig storeConfig)
        {
        }

        private void OnSellStoreLoadingComplete(AsyncOperationHandle<IList<StoreConfig>> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (StoreConfig storeConfig in handle.Result)
                {
                }

                IsLoaded = true;
                OnLoadComplete?.Invoke();
            }
        }
    }
}