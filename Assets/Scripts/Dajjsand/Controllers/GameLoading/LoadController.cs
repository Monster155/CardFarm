using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dajjsand.Controllers.GameLoading
{
    public class LoadController : ILoadController
    {
        public event Action OnAllLoaded;
        public event Action<float> OnPercentageChanged;

        public bool IsAllLoaded { get; private set; } = false;

        public delegate void LoadingStatus(bool isLoaded);

        private LoadingStatus _loadingStatus;
        private List<ILoadable> _loadables = new List<ILoadable>();

        public LoadController() { }

        ~LoadController()
        {
            foreach (ILoadable l in _loadables)
                l.OnLoadComplete -= Loadable_OnLoadComplete;
            _loadables.Clear();
        }

        public void AddLoadable(ILoadable loadable)
        {
            _loadables.Add(loadable);
            loadable.OnLoadComplete += Loadable_OnLoadComplete;
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