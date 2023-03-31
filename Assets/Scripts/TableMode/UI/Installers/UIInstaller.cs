using Zenject;

namespace TableMode
{
    public class UIInstaller : MonoInstaller
    {
        public UIPrefab _uiBehavior;
        public UIAspect _uiAspect;
        public UIAspect _uiAntiAspect;
        
        public override void InstallBindings()
        {
            Container.Bind<IUIController>()
                .To<UIController>()
                .AsSingle()
                .WithArguments(_uiAspect, _uiAntiAspect)
                .NonLazy();
            
            Container.Bind<IUIBehavior>()
                .FromComponentInNewPrefab(_uiBehavior)
                .AsSingle();
        }
        
        
        
    }
}