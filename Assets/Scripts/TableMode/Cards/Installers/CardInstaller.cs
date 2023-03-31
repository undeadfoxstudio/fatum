using UnityEngine;
using Zenject;

namespace TableMode
{
    public class CardInstaller : MonoInstaller
    {
        public BoxCollider InteractableZoneCollider;
        public Transform HandTransform;
        public EntityCardBehavior entityCardBehaviorPrefab;
        public ActionCardBehavior actionCardBehaviorPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<ICardViewFactory>()
                .To<CardViewFactory>()
                .AsSingle()
                .WithArguments(entityCardBehaviorPrefab, actionCardBehaviorPrefab, HandTransform);

            Container.Bind<ICardFactory>()
                .To<CardFactory>()
                .AsSingle();
            
            Container.Bind<IHoverEntityCardsController>()
                .To<HoverEntityCardsController>()
                .AsSingle()
                .NonLazy();

            Container.Bind<IHoverActionCardsController>()
                .To<HoverActionCardsController>()
                .AsSingle()
                .NonLazy();
            
            Container.BindInterfacesTo<MoveEntityCardsController>()
                .AsSingle()
                .WithArguments(InteractableZoneCollider)
                .NonLazy();

            Container.BindInterfacesTo<MoveActionCardsController>()
                .AsSingle()
                .WithArguments(InteractableZoneCollider)
                .NonLazy();
        }
    }
}