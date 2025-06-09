using System;

namespace Dajjsand.Controllers.Game.LoadingController
{
    public interface ILoadController
    {
        public event Action OnAllLoaded;
        public bool IsAllLoaded { get; }
    }
}