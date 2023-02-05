using Zenject;

namespace TableMode
{
    public class AspectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IAspectFactory>()
                .To<AspectFactory>()
                .AsSingle()
                .NonLazy();

            Container.Bind<IAspectController>()
                .To<AspectRuleController>()
                .AsSingle();
        }
    }
}