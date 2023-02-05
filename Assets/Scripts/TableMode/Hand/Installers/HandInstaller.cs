using Zenject;

namespace TableMode.Installers
{
    public class HandInstaller : MonoInstaller
    {
        public HandConfig handConfig;

        public override void InstallBindings()
        {
            Container.Bind<IHandController>()
                .To<HandController>()
                .AsSingle()
                .WithArguments(handConfig);
        }
    }
}