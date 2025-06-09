using Dajjsand.Controllers.Game.Managers.Save;
using Zenject;

namespace Dajjsand.MonoInstallers
{
    public class ProjectMonoInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SaveManager>().AsSingle().NonLazy();
        }
    }
}