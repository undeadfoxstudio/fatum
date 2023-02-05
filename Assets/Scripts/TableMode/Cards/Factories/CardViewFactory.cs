using System;
using UnityEngine;

namespace TableMode
{
    public class CardViewFactory : ICardViewFactory
    {
        private readonly EntityCardBehavior _entityCardPrefab;
        private readonly ActionCardBehavior _actionCardPrefab;
        private readonly IMergeStorage _mergeStorage;
        private readonly Transform _handTransform;
        private readonly ICardFactory _cardFactory;

        public event Action<IEntityCardView> OnCreateEntityCard;
        public event Action<IActionCardView> OnCreateActionCard;

        public CardViewFactory(
            EntityCardBehavior entityCardPrefab,
            ActionCardBehavior actionCardBehavior,
            IMergeStorage mergeStorage,
            Transform handTransform,
            ICardFactory cardFactory)
        {
            _actionCardPrefab = actionCardBehavior;
            _mergeStorage = mergeStorage;
            _entityCardPrefab = entityCardPrefab;
            _handTransform = handTransform;
            _cardFactory = cardFactory;
        }

        public IEntityCardView CreateEntityCard(string entityId)
        {
            var cardBehavior = UnityEngine.Object.Instantiate(_entityCardPrefab);
            var entityCard = _cardFactory.CreateEntityCard(entityId);
            var entityCardView = new EntityCardView(_mergeStorage, cardBehavior, entityCard);

            OnCreateEntityCard?.Invoke(entityCardView);

            return entityCardView;
        }

        public IActionCardView CreateActionCard(string actionId)
        {
            var cardBehavior = UnityEngine.Object.Instantiate(_actionCardPrefab, _handTransform);
            var actionCard = _cardFactory.CreateActionCard(actionId);
            var actionCardView = new ActionCardView(cardBehavior, actionCard, _mergeStorage);

            OnCreateActionCard?.Invoke(actionCardView);

            return actionCardView;
        }
    }
}