using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TableMode
{
    public class CardViewFactory : ICardViewFactory
    {
        private readonly EntityCardBehavior _entityCardPrefab;
        private readonly ActionCardBehavior _actionCardPrefab;
        private readonly IContentProvider _contentProvider;
        private readonly IAssetsProvider _assetsProvider;
        private readonly IMergeStorage _mergeStorage;
        private readonly ITextureGenerator _textureGenerator;
        private readonly Transform _handTransform;
        private readonly ICardFactory _cardFactory;
        private readonly IAspectViewFactory _aspectViewFactory;

        public event Action<IEntityCardView> OnCreateEntityCard;
        public event Action<IActionCardView> OnCreateActionCard;

        public CardViewFactory(
            EntityCardBehavior entityCardPrefab,
            ActionCardBehavior actionCardBehavior,
            IContentProvider contentProvider,
            IAssetsProvider assetsProvider,
            IMergeStorage mergeStorage,
            ITextureGenerator textureGenerator,
            Transform handTransform,
            ICardFactory cardFactory,
            IAspectViewFactory aspectViewFactory)
        {
            _actionCardPrefab = actionCardBehavior;
            _contentProvider = contentProvider;
            _assetsProvider = assetsProvider;
            _mergeStorage = mergeStorage;
            _textureGenerator = textureGenerator;
            _entityCardPrefab = entityCardPrefab;
            _handTransform = handTransform;
            _cardFactory = cardFactory;
            _aspectViewFactory = aspectViewFactory;
        }

        public IEntityCardView CreateEntityCard(string entityId)
        {
            var asset = _contentProvider.GetEntityById(entityId).Asset;
            var cardBehavior = UnityEngine.Object.Instantiate(_entityCardPrefab);
            
            if (!string.IsNullOrEmpty(asset))
                cardBehavior.SetSprite(_assetsProvider.GetCardSpritePack(asset));

            var entityCard = _cardFactory.CreateEntityCard(entityId);
            var aspects = CreateAspects(entityCard.Aspects);
            var antiAspects = CreateAntiAspects(entityCard.AntiAspects);

            var entityCardView = new EntityCardView(
                _mergeStorage, 
                _textureGenerator,
                cardBehavior, 
                entityCard,
                aspects,
                antiAspects);

            OnCreateEntityCard?.Invoke(entityCardView);

            return entityCardView;
        }

        public IActionCardView CreateActionCard(string actionId)
        {
            var asset = _contentProvider.GetActionById(actionId).Asset;
            var cardBehavior = UnityEngine.Object.Instantiate(_actionCardPrefab, _handTransform);
            
            if (!string.IsNullOrEmpty(asset))
                cardBehavior.SetSprite(_assetsProvider.GetCardSpritePack(asset));

            var actionCard = _cardFactory.CreateActionCard(actionId);
            var aspects = CreateAspects(actionCard.Aspects);
            var antiAspects = CreateAspects(actionCard.AntiAspects);

            var actionCardView = new ActionCardView(
                cardBehavior, 
                _textureGenerator,
                actionCard, 
                _mergeStorage,
                aspects,
                antiAspects);

            OnCreateActionCard?.Invoke(actionCardView);

            return actionCardView;
        }

        private IEnumerable<IAspectView> CreateAspects(IEnumerable<IAspect> aspects) =>
            aspects.Select(aspect => _aspectViewFactory.CreateAspect(aspect.Id, aspect.Count)).ToList();

        private IEnumerable<IAspectView> CreateAntiAspects(IEnumerable<IAspect> aspects) =>
            aspects.Select(aspect => _aspectViewFactory.CreateAntiAspect(aspect.Id, aspect.Count)).ToList();
    }
}