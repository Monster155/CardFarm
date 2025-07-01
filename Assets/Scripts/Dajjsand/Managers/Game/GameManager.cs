using Dajjsand.Controllers.GameLoading;
using Dajjsand.Enums;
using Dajjsand.Factories.CardFactory;
using Dajjsand.Factories.LevelConfigFactory;
using Dajjsand.Managers.Save;
using Dajjsand.ScriptableObjects;

namespace Dajjsand.Managers.Game
{
    public class GameManager
    {
        private ILoadController _loadController;
        private ICardFactory _cardFactory;
        private ILevelConfigFactory _levelConfigFactory;
        private ISaveManager _saveManager;

        private LevelConfig _currentLevelConfig;

        public GameManager(ILoadController loadController, ICardFactory cardFactory,
            ILevelConfigFactory levelConfigFactory, ISaveManager saveManager)
        {
            _loadController = loadController;
            _cardFactory = cardFactory;
            _levelConfigFactory = levelConfigFactory;
            _saveManager = saveManager;

            _loadController.OnAllLoaded += LoadController_OnAllLoaded;
        }

        ~GameManager()
        {
            _loadController.OnAllLoaded -= LoadController_OnAllLoaded;
        }

        private void LoadController_OnAllLoaded()
        {
            _currentLevelConfig = _levelConfigFactory.GetLevelConfig(_saveManager.GetCurrentLevelIndex());

            foreach (CraftIngredient ingredient in _currentLevelConfig._startIngredients)
            {
                var card = _cardFactory.GetCard(ingredient);
            }
        }
    }
}