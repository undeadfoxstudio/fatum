using Zenject;

namespace TableMode
{
    public class AssetInstaller : MonoInstaller
    {
        public AssetsConfig assetsConfig;
        
        public override void InstallBindings()
        {
            Container.Bind<IAssetsProvider>()
                .To<AssetsProvider>()
                .AsSingle()
                .WithArguments(assetsConfig);
        }
    }
}
