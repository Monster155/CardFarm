using Dajjsand.Controllers.GameLoading;
using Dajjsand.Factories.CardFactory;
using Dajjsand.Factories.LevelConfigFactory;
using Dajjsand.Handlers;
using Dajjsand.Handlers.SceneLoad;
using Dajjsand.Managers.Game;
using Dajjsand.Managers.Save;
using UnityEngine;
using Zenject;

namespace Dajjsand.MonoInstallers
{
    public class GameSceneMonoInstaller : MonoInstaller
    {
        [SerializeField] private ContainersHandler _containersHandler;
        
        public override void InstallBindings()
        {
            Container.Bind<GameManager>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<CardFactory>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SaveManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LoadController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LevelConfigFactory>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SceneLoadHandler>().AsSingle().NonLazy();

            Container.BindInstance(_containersHandler).AsSingle().NonLazy();
        }
    }
}