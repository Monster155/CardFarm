using Dajjsand.Controllers.Game.Controllers.Loading;
using Dajjsand.Controllers.Game.Factories.LevelConfigFactory;
using Dajjsand.Controllers.Game.Handlers.SceneLoad;
using Dajjsand.Controllers.Game.Managers.Save;
using Zenject;

namespace Dajjsand.MonoInstallers
{
    public class ProjectMonoInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SaveManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LoadController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LevelConfigFactory>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SceneLoadHandler>().AsSingle().NonLazy();
        }
    }
}