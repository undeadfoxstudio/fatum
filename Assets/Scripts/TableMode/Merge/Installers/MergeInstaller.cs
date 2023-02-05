using Zenject;

namespace TableMode
{
    public class MergeInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IMergeStorage>()
                .To<MergeStorage>()
                .AsSingle();

            Container.Bind<MergeController>()
                .AsSingle()
                .NonLazy();
        }
    }
}