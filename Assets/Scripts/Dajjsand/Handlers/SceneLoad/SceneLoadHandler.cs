using System;
using Cysharp.Threading.Tasks;
using Dajjsand.Controllers.GameLoading;
using Tymski;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dajjsand.Handlers.SceneLoad
{
    public class SceneLoadHandler : ISceneLoadHandler
    {
        public event Action OnLoadComplete;
        public bool IsLoaded { get; private set; } = true;

        public SceneLoadHandler(ILoadController loadController)
        {
            loadController.AddLoadable(this);
        }

        public async UniTask LoadSceneAsync(SceneReference scene)
        {
            IsLoaded = false;

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

            await asyncLoad.ToUniTask();

            IsLoaded = true;
            OnLoadComplete?.Invoke();
        }
    }
}