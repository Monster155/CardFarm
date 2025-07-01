using System;

namespace Dajjsand.Controllers.GameLoading
{
    public interface ILoadable
    {
        public event Action OnLoadComplete;
        public bool IsLoaded { get; }
    }
}