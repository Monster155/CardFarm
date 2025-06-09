using Dajjsand.Controllers.Game.CardFactory;
using Dajjsand.Controllers.Game.LoadingController;
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
            Container.BindInterfacesAndSelfTo<CardFactory>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LoadController>().AsSingle().NonLazy();

            Container.BindInstance(_containersHandler).AsSingle().NonLazy();
        }
    }
}