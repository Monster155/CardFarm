using System;
using System.Collections.Generic;
using Dajjsand.Controllers.GameLoading;
using Dajjsand.Factories.LevelConfigFactory;
using Dajjsand.Handlers.SceneLoad;
using Dajjsand.Managers.Save;
using Dajjsand.View.Menu.SelectLevel;
using Tymski;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Dajjsand.View.Menu
{
    public class SelectLevelScreen : BaseScreen
    {
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private SceneReference _gameScene;
        [Space]
        [SerializeField] private LevelItem _levelItemPrefab;
        [SerializeField] private Transform _levelItemContainer;
        [Space]
        [SerializeField] private Button _backButton;

        private ILevelConfigFactory _levelConfigFactory;
        private ISaveManager _saveManager;
        private ISceneLoadHandler _sceneLoadHandler;
        private ILoadController _loadController;

        private List<LevelItem> _levelItems = new List<LevelItem>();

        [Inject]
        private void Construct(ILevelConfigFactory levelConfigFactory, ISaveManager saveManager,
            ISceneLoadHandler sceneLoadHandler, ILoadController loadController)
        {
            _levelConfigFactory = levelConfigFactory;
            _saveManager = saveManager;
            _sceneLoadHandler = sceneLoadHandler;
            _loadController = loadController;
        }

        private void Start()
        {
            int levelsCount = _levelConfigFactory.GetLevelsCount();
            int maxReachedLevelIndex = _saveManager.GetMaxReachedLevelIndex();
            int[] starsByLevelIndex = _saveManager.GetAllStarsByLevel();

            for (int levelIndex = 0; levelIndex < levelsCount; levelIndex++)
            {
                var levelItem = Instantiate(_levelItemPrefab, _levelItemContainer);
                levelItem.OnClick += LevelItem_OnClick;
                levelItem.Init(levelIndex,
                    levelIndex < starsByLevelIndex.Length
                        ? starsByLevelIndex[levelIndex]
                        : 0,
                    levelIndex <= maxReachedLevelIndex);

                _levelItems.Add(levelItem);
            }

            _loadController.OnAllLoaded += LoadController_OnAllLoaded;
            _loadController.OnPercentageChanged += LoadController_OnPercentageChanged;

            _backButton.onClick.AddListener(BackButton_OnClick);
        }

        private void OnDestroy()
        {
            _loadController.OnAllLoaded -= LoadController_OnAllLoaded;
            _loadController.OnPercentageChanged -= LoadController_OnPercentageChanged;
            
            _backButton.onClick.RemoveListener(BackButton_OnClick);
        }

        private void OnEnable()
        {
            int maxReachedLevelIndex = _saveManager.GetMaxReachedLevelIndex();
            int[] starsByLevelIndex = _saveManager.GetAllStarsByLevel();

            for (int levelIndex = 0; levelIndex < _levelItems.Count; levelIndex++)
            {
                _levelItems[levelIndex].UpdateContent(
                    levelIndex < starsByLevelIndex.Length
                        ? starsByLevelIndex[levelIndex]
                        : 0,
                    levelIndex <= maxReachedLevelIndex);
            }
        }

        private void StartLevel(int levelIndex)
        {
            _saveManager.SaveCurrentLevelIndex(levelIndex);
            _loadingScreen.Show();
            _ = _sceneLoadHandler.LoadSceneAsync(_gameScene);
        }

        private void LoadingFinished()
        {
            _loadingScreen.Hide();
        }

        private void UpdateLoadingProgress(float percent)
        {
            _loadingScreen.UpdateProgress(percent);
        }

        private void LevelItem_OnClick(int levelIndex) => StartLevel(levelIndex);
        private void LoadController_OnAllLoaded() => LoadingFinished();
        private void LoadController_OnPercentageChanged(float percent) => UpdateLoadingProgress(percent);
        private void BackButton_OnClick() => Hide();
    }
}