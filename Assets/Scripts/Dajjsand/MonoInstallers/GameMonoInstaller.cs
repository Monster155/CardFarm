using Dajjsand.Controllers.Game;
using Dajjsand.Controllers.Game.Factories.CardFactory;
using Dajjsand.Controllers.Game.Factories.LevelConfigFactory;
using Dajjsand.Controllers.Game.LoadingController;
using Dajjsand.Controllers.Game.Managers.Save;
using Dajjsand.Controllers.Game.Utils;
using UnityEngine;
using Zenject;

namespace Dajjsand.MonoInstallers
{
    public class GameMonoInstaller : MonoInstaller
    {
        [SerializeField] private ContainersHandler _containersHandler;
        
        public override void InstallBindings()
        {
            Container.Bind<GameManager>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<CardFactory>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LoadController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LevelConfigFactory>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SaveManager>().AsSingle().NonLazy();

            Container.BindInstance(_containersHandler).AsSingle().NonLazy();
        }
    }
}