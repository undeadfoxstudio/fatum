using Zenject;

namespace TableMode.Installers
{
    public class StepInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IStepController>()
                .To<StepController>()
                .AsSingle()
                .NonLazy();
        }
    }
}