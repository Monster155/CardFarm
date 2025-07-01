using System;

namespace Dajjsand.Controllers.GameLoading
{
    public interface ILoadController
    {
        public event Action OnAllLoaded;
        public event Action<float> OnPercentageChanged;
        public bool IsAllLoaded { get; }
    }
}