using UnityEngine;
using Zenject;

namespace TableMode
{
    public class AspectInstaller : MonoInstaller
    {
        [SerializeField] private AspectBehavior _aspectBehavior;
        
        public override void InstallBindings()
        {
            Container.Bind<IAspectFactory>()
                .To<AspectFactory>()
                .AsSingle();

            Container.Bind<IAspectRuleProvider>()
                .To<AspectRuleProvider>()
                .AsSingle();

            Container.Bind<IAspectViewFactory>()
                .To<AspectViewFactory>()
                .AsSingle()
                .WithArguments(_aspectBehavior);
        }
    }
}