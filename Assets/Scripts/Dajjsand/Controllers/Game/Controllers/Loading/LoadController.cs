using System;
using System.Collections.Generic;
using System.Linq;

namespace Dajjsand.Controllers.Game.Controllers.Loading
{
    public class LoadController : ILoadController
    {
        public event Action OnAllLoaded;
        public event Action<float> OnPercentageChanged;

        public bool IsAllLoaded { get; private set; }

        public delegate void LoadingStatus(bool isLoaded);

        private LoadingStatus _loadingStatus;
        private List<ILoadable> _loadables;

        public LoadController(List<ILoadable> loadables)
        {
            _loadables = loadables;

            foreach (ILoadable l in _loadables)
                l.OnLoadComplete += Loadable_OnLoadComplete;

            IsAllLoaded = false;
        }

        ~LoadController()
        {
            foreach (ILoadable l in _loadables)
                l.OnLoadComplete -= Loadable_OnLoadComplete;
            _loadables.Clear();
        }

        private void Loadable_OnLoadComplete()
        {
            int loadedCount = _loadables.Count(l => l.IsLoaded);
            OnPercentageChanged?.Invoke((float)loadedCount / _loadables.Count);

            if (loadedCount < _loadables.Count)
                return;

            IsAllLoaded = true;
            OnAllLoaded?.Invoke();
        }
    }
}