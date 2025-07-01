using Dajjsand.Controllers.Game.Controllers.Loading;
using Dajjsand.Controllers.Game.Handlers.SceneLoad;
using Dajjsand.View.Menu;
using Tymski;
using UnityEngine;
using Zenject;

namespace Dajjsand.View.FirstScene
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] private LoadingScreen _loadingScreen;

        private ILoadController _loadController;

        [Inject]
        private void Construct(ILoadController loadController)
        {
            _loadController = loadController;
        }

        private void Start()
        {
            _loadController.OnAllLoaded += LoadController_OnAllLoaded;
            _loadController.OnPercentageChanged += LoadController_OnPercentageChanged;

            if (!_loadController.IsAllLoaded)
                _loadingScreen.Show();
        }

        private void LoadController_OnAllLoaded() => _loadingScreen.Hide();
        private void LoadController_OnPercentageChanged(float percent) => _loadingScreen.UpdateProgress(percent);
    }
}