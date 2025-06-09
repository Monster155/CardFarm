using System;

namespace Dajjsand.Controllers.Game.Controllers.Loading
{
    public interface ILoadable
    {
        public event Action OnLoadComplete;
        public bool IsLoaded { get; }
    }
}