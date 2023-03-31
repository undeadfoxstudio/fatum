using Zenject;

namespace TableMode
{
    public class GeneralInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<UnityEventMediator>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesTo<InputController>()
                .AsSingle()
                .NonLazy();
        }
    }
}