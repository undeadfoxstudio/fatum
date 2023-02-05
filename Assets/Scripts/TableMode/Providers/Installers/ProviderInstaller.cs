using Zenject;

namespace TableMode
{
    public class ProviderInstaller : MonoInstaller
    {
        public ContentConfig contentConfig;
        
        public override void InstallBindings()
        {
            Container.Bind<IContentProvider>()
                .To<ContentProvider>()
                .AsSingle()
                .WithArguments(contentConfig);
        }
    }
}