using System;
using System.Collections.Generic;
using System.Linq;
using Dajjsand.Factories.CardFactory;
using Dajjsand.Factories.LevelConfigFactory;
using Dajjsand.Handlers.SceneLoad;
using Dajjsand.Managers.Save;

namespace Dajjsand.Controllers.GameLoading
{
    public class LoadController : ILoadController
    {
        public event Action OnAllLoaded;
        public event Action<float> OnPercentageChanged;

        public bool IsAllLoaded { get; private set; }

        private List<ILoadable> _loadables = new();

        public LoadController(ICardFactory cardFactory, ILevelConfigFactory levelConfigFactory)
        {
            _loadables.Add(cardFactory);
            _loadables.Add(levelConfigFactory);

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