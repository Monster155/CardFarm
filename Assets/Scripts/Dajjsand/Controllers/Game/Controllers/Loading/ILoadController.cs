using System;

namespace Dajjsand.Controllers.Game.Controllers.Loading
{
    public interface ILoadController
    {
        public event Action OnAllLoaded;
        public event Action<float> OnPercentageChanged;
        public bool IsAllLoaded { get; }
    }
}