﻿using Dajjsand.Controllers.Loading;
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
            var maxReachedLevelIndex = _saveManager.GetMaxReachedLevelIndex();
            int[] starsByLevelIndex = _saveManager.GetStarsByLevelIndex();

            for (int levelIndex = 0; levelIndex < levelsCount; levelIndex++)
            {
                var levelItem = Instantiate(_levelItemPrefab, _levelItemContainer);
                levelItem.OnClick += LevelItem_OnClick;
                levelItem.Init(levelIndex,
                    levelIndex < starsByLevelIndex.Length
                        ? starsByLevelIndex[levelIndex]
                        : 0,
                    levelIndex <= maxReachedLevelIndex);
            }

            _loadController.OnAllLoaded += LoadController_OnAllLoaded;
            _loadController.OnPercentageChanged += LoadController_OnPercentageChanged;

            _backButton.onClick.AddListener(BackButton_OnClick);
        }

        private void StartLevel(int levelIndex)
        {
            _saveManager.SaveCurrentLevelIndex(levelIndex);
            _loadingScreen.Show();
            _ = _sceneLoadHandler.LoadSceneAsync(_gameScene);
        }

        private void LevelItem_OnClick(int levelIndex) => StartLevel(levelIndex);
        private void LoadController_OnAllLoaded() => _loadingScreen.Hide();
        private void LoadController_OnPercentageChanged(float percent) => _loadingScreen.UpdateProgress(percent);
        private void BackButton_OnClick() => Hide();
    }
}