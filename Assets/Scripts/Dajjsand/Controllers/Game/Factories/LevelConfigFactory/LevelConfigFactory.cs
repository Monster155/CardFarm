using System;
using System.Collections.Generic;
using System.Linq;
using Dajjsand.Controllers.Game.Controllers.Loading;
using Dajjsand.Controllers.Game.ScriptableObjects;
using Dajjsand.Controllers.Game.Utils.Constants;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Dajjsand.Controllers.Game.Factories.LevelConfigFactory
{
    public class LevelConfigFactory : ILevelConfigFactory, ILoadable
    {
        public event Action OnLoadComplete;
        public bool IsLoaded { get; private set; }

        private Dictionary<int, LevelConfig> _levelsConfigs;
        private AsyncOperationHandle<IList<LevelConfig>> _levelsConfigsLoadingHandle;

        public LevelConfigFactory()
        {
            _levelsConfigsLoadingHandle = Addressables.LoadAssetsAsync<LevelConfig>(AddressablePathConstants.LevelsConfigs);
            _levelsConfigsLoadingHandle.Completed += LevelsConfigsLoadingHandle_Completed;
        }

        public LevelConfig GetLevelConfig(int levelIndex)
        {
            bool isValueGot = _levelsConfigs.TryGetValue(levelIndex, out LevelConfig levelConfig);
            if (!isValueGot)
                Debug.LogError($"Level config for level {levelIndex} doesn't found!");
            return levelConfig;
        }

        private void LevelsConfigsLoadingHandle_Completed(AsyncOperationHandle<IList<LevelConfig>> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _levelsConfigs = new Dictionary<int, LevelConfig>();
                foreach (LevelConfig config in handle.Result)
                {
                    _levelsConfigs.Add(config.LevelIndex, config);
                    Debug.Log($"Loaded Level config for level {config.LevelIndex}");
                }

                IsLoaded = true;
                OnLoadComplete?.Invoke();
            }
        }
    }
}