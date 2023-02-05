using Zenject;

namespace TableMode
{
    public class GeneralInstaller : MonoInstaller
    {
        public UIPrefab uiPrefab;

        public override void InstallBindings()
        {
            Container.Bind<UnityEventMediator>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesTo<InputController>()
                .AsSingle()
                .NonLazy();

            Container.Bind<IUIBehavior>()
                .FromComponentInNewPrefab(uiPrefab)
                .AsSingle();
        }
    }
}