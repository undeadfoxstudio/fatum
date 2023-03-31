using Zenject;

namespace TableMode
{
    public class InspectorInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IInspectorController>()
                .To<InspectorController>()
                .AsSingle()
                .NonLazy();
        }
    }
}