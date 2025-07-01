using System;
using System.Collections.Generic;
using Dajjsand.Controllers.Loading;
using Dajjsand.ScriptableObjects;
using Dajjsand.Utils.Constants;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Dajjsand.Factories.LevelConfigFactory
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
            int levelNumber = levelIndex + 1; // level indexes start from 0, level numbers start from 1
            bool isValueGot = _levelsConfigs.TryGetValue(levelNumber, out LevelConfig levelConfig);
            if (!isValueGot)
                Debug.LogError($"Level config for level {levelNumber} doesn't found!");
            return levelConfig;
        }

        public int GetLevelsCount() => _levelsConfigs.Count;

        private void LevelsConfigsLoadingHandle_Completed(AsyncOperationHandle<IList<LevelConfig>> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _levelsConfigs = new Dictionary<int, LevelConfig>();
                foreach (LevelConfig config in handle.Result)
                {
                    _levelsConfigs.Add(config._levelNumber, config);
                    Debug.Log($"Loaded Level config for level {config._levelNumber}");
                }

                IsLoaded = true;
                OnLoadComplete?.Invoke();
            }
        }
    }
}