using Dajjsand.ScriptableObjects;
using UnityEngine;

namespace Dajjsand.View.Game.Store
{
    public class StoreCard : MonoBehaviour
    {
        [SerializeField] private Transform _container;

        public void InitSellStore(SellStoreConfig sellStoreConfig)
        {
        }

        public void InitDefaultStore(StoreConfig storeConfig)
        {
        }
    }
}