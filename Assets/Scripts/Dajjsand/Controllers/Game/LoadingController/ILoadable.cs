using System;

namespace Dajjsand.Controllers.Game.LoadingController
{
    public interface ILoadable
    {
        public event Action OnLoadComplete;
        public bool IsLoaded { get; }
    }
}