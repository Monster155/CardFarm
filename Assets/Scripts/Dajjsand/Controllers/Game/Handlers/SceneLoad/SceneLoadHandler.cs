using System;
using Cysharp.Threading.Tasks;
using Dajjsand.Controllers.Game.Controllers.Loading;
using Tymski;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dajjsand.Controllers.Game.Handlers.SceneLoad
{
    public class SceneLoadHandler : ISceneLoadHandler,ILoadable
    {
        public event Action OnLoadComplete;
        public bool IsLoaded { get; private set; }

        public async UniTask LoadSceneAsync(SceneReference scene)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

            await asyncLoad.ToUniTask();

            IsLoaded = true;
            OnLoadComplete?.Invoke();
        }
    }
}