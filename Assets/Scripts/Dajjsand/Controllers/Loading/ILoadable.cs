using System;

namespace Dajjsand.Controllers.Loading
{
    public interface ILoadable
    {
        public event Action OnLoadComplete;
        public bool IsLoaded { get; }
    }
}