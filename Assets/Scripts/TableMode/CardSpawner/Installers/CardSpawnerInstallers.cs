using Zenject;

namespace TableMode
{
    public class CardSpawnerInstallers : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ICardSpawner>()
                .To<CardSpawner>()
                .AsSingle();
        }
    }
}