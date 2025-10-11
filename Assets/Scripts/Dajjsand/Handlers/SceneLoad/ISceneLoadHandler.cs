using Cysharp.Threading.Tasks;
using Dajjsand.Controllers.GameLoading;
using Tymski;

namespace Dajjsand.Handlers.SceneLoad
{
    public interface ISceneLoadHandler : ILoadable
    {
        public UniTask LoadSceneAsync(SceneReference scene);
    }
}