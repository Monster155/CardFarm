using Cysharp.Threading.Tasks;
using Tymski;

namespace Dajjsand.Controllers.Game.Handlers.SceneLoad
{
    public interface ISceneLoadHandler
    {
        public UniTask LoadSceneAsync(SceneReference scene);
    }
}