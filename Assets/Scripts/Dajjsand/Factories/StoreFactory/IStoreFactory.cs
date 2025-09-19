using System.Collections.Generic;
using Dajjsand.Controllers.GameLoading;
using Dajjsand.ScriptableObjects;

namespace Dajjsand.Factories.StoreFactory
{
    public interface IStoreFactory : ILoadable
    {
        void SpawnSellStore();
        void SpawnStores(List<StoreConfig> storeConfigs);
        void SpawnStore(StoreConfig storeConfig);
    }
}