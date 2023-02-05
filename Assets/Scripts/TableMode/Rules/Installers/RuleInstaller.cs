using Zenject;

namespace TableMode.Installers
{
    public class RuleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IRuleController>()
                .To<RuleController>()
                .AsSingle();
        }
    }
}