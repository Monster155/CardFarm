using Dajjsand.Controllers.Game.Factories.CardFactory;
using Dajjsand.Controllers.Game.Factories.LevelConfigFactory;
using Dajjsand.Controllers.Game.LoadingController;
using Dajjsand.Controllers.Game.Managers.Save;

namespace Dajjsand.Controllers.Game
{
    public class GameManager
    {
        private ILoadController _loadController;
        private ICardFactory _cardFactory;
        private ILevelConfigFactory _levelConfigFactory;
        private ISaveManager _saveManager;

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
            _levelConfigFactory.GetLevelConfig(_saveManager.GetCurrentLevelIndex());
        }
    }
}