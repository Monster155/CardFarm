using Dajjsand.Controllers.Game.Controllers.Loading;
using UnityEngine;
using Zenject;

namespace Dajjsand.View_UI
{
    public class LoadingPage : MonoBehaviour
    {
        private LoadController _loadController;

        [Inject]
        private void Construct(LoadController loadController)
        {
            _loadController = loadController;
            _loadController.OnAllLoaded += LoadController_OnAllLoaded;
        }

        private void LoadController_OnAllLoaded()
        {
            gameObject.SetActive(false);
        }
    }
}