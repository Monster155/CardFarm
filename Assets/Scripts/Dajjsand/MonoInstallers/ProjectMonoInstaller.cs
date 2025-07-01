using Dajjsand.Controllers.Loading;
using Dajjsand.Factories.LevelConfigFactory;
using Dajjsand.Handlers.SceneLoad;
using Dajjsand.Managers.Save;
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