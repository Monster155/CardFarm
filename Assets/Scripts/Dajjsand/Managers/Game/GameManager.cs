using System.Collections.Generic;
using Dajjsand.Controllers.Craft;
using Dajjsand.Controllers.GameLoading;
using Dajjsand.Controllers.Tasks;
using Dajjsand.Enums;
using Dajjsand.Factories.CardFactory;
using Dajjsand.Factories.LevelConfigFactory;
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

        private LevelConfig _currentLevelConfig;

        private List<BaseCard> _spawnedFromStarterPackCards = new();

        public GameManager(ILoadController loadController, ICardFactory cardFactory,
            ILevelConfigFactory levelConfigFactory, ISaveManager saveManager,
            ITasksController tasksController)
        {
            _loadController = loadController;
            _cardFactory = cardFactory;
            _levelConfigFactory = levelConfigFactory;
            _saveManager = saveManager;
            _tasksController = tasksController;

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

            var card = _cardFactory.GetStarterPack(_currentLevelConfig._startIngredients);
            card.SetDraggingLockedState(true);
            card.OnClick += PackCard_OnClick;
        }

        private void TasksController_OnAllTasksFinished()
        {
            CraftController.Instance.Dispose();
        }

        private void PackCard_OnClick(BaseCard packCard)
        {
            CardType? card = packCard.GetCardFromContainer();
            if (card != null)
            {
                var newCard = _cardFactory.GetCard((CardType)card, packCard.transform.position + new Vector3(0.2f, 0, 0.2f));
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