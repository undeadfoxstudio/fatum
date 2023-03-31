using Zenject;

namespace TableMode
{
    public class CardSpawnerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ICardSpawner>()
                .To<CardSpawner>()
                .AsSingle();
        }
    }
}