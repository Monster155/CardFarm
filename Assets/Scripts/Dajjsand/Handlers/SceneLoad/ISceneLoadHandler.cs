using Cysharp.Threading.Tasks;
using Tymski;

namespace Dajjsand.Handlers.SceneLoad
{
    public interface ISceneLoadHandler
    {
        public UniTask LoadSceneAsync(SceneReference scene);
    }
}