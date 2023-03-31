using Zenject;

namespace TableMode.TableMode.Generators.Installers
{
    public class GeneratorsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ITextureGenerator>()
                .To<TextureGenerator>()
                .AsSingle();
        }
    }
}