using System.Collections.Generic;
using Dajjsand.Controllers.Craft;
using Dajjsand.Controllers.GameLoading;
using Dajjsand.Controllers.Tasks;
using Dajjsand.Enums;
using Dajjsand.Factories.CardFactory;
using Dajjsand.Factories.LevelConfigFactory;
using Dajjsand.Factories.StoreFactory;
using Dajjsand.Managers.Save;
using Dajjsand.ScriptableObjects;
using Dajjsand.View.Game.Cards;
using UnityEngine;

namespace Dajjsand.Managers.Game
{
    public class GameManager
    {
        private ILoadController _loadController;
        private ICardFactory _cardFactory;
        private ILevelConfigFactory _levelConfigFactory;
        private ISaveManager _saveManager;
        private ITasksController _tasksController;
        private IStoreFactory _storeFactory;

        private LevelConfig _currentLevelConfig;

        private List<BaseCard> _spawnedFromStarterPackCards = new();

        public GameManager(ILoadController loadController, ICardFactory cardFactory,
            ILevelConfigFactory levelConfigFactory, ISaveManager saveManager,
            ITasksController tasksController, IStoreFactory storeFactory)
        {
            _loadController = loadController;
            _cardFactory = cardFactory;
            _levelConfigFactory = levelConfigFactory;
            _saveManager = saveManager;
            _tasksController = tasksController;
            _storeFactory = storeFactory;

            _loadController.OnAllLoaded += LoadController_OnAllLoaded;
            _tasksController.OnAllTasksFinished += TasksController_OnAllTasksFinished;
        }

        ~GameManager()
        {
            _loadController.OnAllLoaded -= LoadController_OnAllLoaded;
            _tasksController.OnAllTasksFinished -= TasksController_OnAllTasksFinished;
        }

        private void LoadController_OnAllLoaded()
        {
            _currentLevelConfig = _levelConfigFactory.GetLevelConfig(_saveManager.GetCurrentLevelIndex());

            // initiating singleton
            CraftController craftController = new CraftController(_currentLevelConfig._availableRecipes, _cardFactory);

            // starter packs
            var starterPacks = _cardFactory.GetPacks(_currentLevelConfig._starterPacks);
            foreach (var pack in starterPacks)
            {
                pack.SetDraggingLockedState(true);
                pack.OnClick += PackCard_OnClick;
            }

            // stores
            _storeFactory.SpawnSellStore();
            _storeFactory.SpawnStores(_currentLevelConfig._storeConfigs);
        }

        private void TasksController_OnAllTasksFinished()
        {
            CraftController.Instance.Dispose();
        }

        private void PackCard_OnClick(BaseCard packCard)
        {
            CardType? card = packCard.GetCardFromContainer(out Vector3 offset);
            if (card != null)
            {
                var newCard = _cardFactory.GetCard((CardType)card, packCard.transform.position + offset);
                newCard.SetDraggingLockedState(true);
                _spawnedFromStarterPackCards.Add(newCard);
            }

            if (!packCard.IsAnyCardInContainer())
            {
                packCard.OnClick -= PackCard_OnClick;
                _cardFactory.ReleaseCard(packCard);
                foreach (BaseCard newCard in _spawnedFromStarterPackCards)
                    newCard.SetDraggingLockedState(false);
            }
        }
    }
}