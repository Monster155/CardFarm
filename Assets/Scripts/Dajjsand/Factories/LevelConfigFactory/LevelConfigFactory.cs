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
    public class LevelConfigFactory : ILevelConfigFactory
    {
        public event Action OnLoadComplete;
        public bool IsLoaded { get; private set; }

        private bool IsAllLoaded => _testLevelConfigLoadingHandle.IsDone && _levelsConfigsLoadingHandle.IsDone;

        private Dictionary<int, LevelConfig> _levelsConfigs;
        private AsyncOperationHandle<IList<LevelConfig>> _levelsConfigsLoadingHandle;
        private LevelConfig _testLevelConfig;
        private AsyncOperationHandle<LevelConfig> _testLevelConfigLoadingHandle;

        public LevelConfigFactory()
        {
            _levelsConfigsLoadingHandle = Addressables.LoadAssetsAsync<LevelConfig>(AddressablePathConstants.LevelsConfigs);
            _levelsConfigsLoadingHandle.Completed += LevelsConfigsLoadingHandle_Completed;
            _testLevelConfigLoadingHandle = Addressables.LoadAssetAsync<LevelConfig>(AddressablePathConstants.TestLevelConfig);
            _testLevelConfigLoadingHandle.Completed += TestLevelConfigLoadingHandle_Completed;
        }

        public LevelConfig GetLevelConfig(int levelIndex)
        {
            if (levelIndex < 0)
            {
                Debug.Log("Loading test level...");
                return _testLevelConfig;
            }

            int levelNumber = levelIndex + 1; // level indexes start from 0, level numbers start from 1
            bool isValueGot = _levelsConfigs.TryGetValue(levelNumber, out LevelConfig levelConfig);
            if (!isValueGot)
                Debug.LogError($"Level config for level {levelNumber} doesn't found!");
            return levelConfig;
        }

        public int GetLevelsCount() => _levelsConfigs.Count;

        private void UpdateLoadingState()
        {
            if (IsAllLoaded)
            {
                IsLoaded = true;
                OnLoadComplete?.Invoke();
            }
        }

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
            }
            else Debug.LogError($"LevelsConfigsLoadingHandle finished with {handle.Status} status");

            UpdateLoadingState();
        }

        private void TestLevelConfigLoadingHandle_Completed(AsyncOperationHandle<LevelConfig> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _testLevelConfig = handle.Result;
            }
            else Debug.LogError($"TestLevelConfigLoadingHandle finished with {handle.Status} status");

            UpdateLoadingState();
        }
    }
}