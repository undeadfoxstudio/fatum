using System.Collections.Generic;
using System.Linq;

namespace TableMode
{
    public class CardFactory : ICardFactory
    {
        private readonly IContentProvider _contentProvider;
        private readonly IAspectFactory _aspectFactory;

        public CardFactory(
            IContentProvider contentProvider,
            IAspectFactory aspectFactory)
        {
            _contentProvider = contentProvider;
            _aspectFactory = aspectFactory;
        }

        public IActionCard CreateActionCard(string actionId)
        {
            var cardModel = _contentProvider.GetActionById(actionId);

            var cardAspects = cardModel.Aspects
                .Select(CreateAspect)
                .ToList();
            var cardAntiAspects = cardModel.AntiAspects
                .Select(CreateAspect)
                .ToList();

            return new ActionCard(
                actionId,
                cardAspects,
                cardAntiAspects,
                cardModel.Name,
                cardModel.Description);
        }

        public IEntityCard CreateEntityCard(string entityId)
        {
            var cardModel = _contentProvider.GetEntityById(entityId);

            var cardAspects = cardModel.Aspects
                .Select(CreateAspect)
                .ToList();

            var cardAntiAspects = cardModel.AntiAspects
                .Select(CreateAspect)
                .ToList();

            var ent = new EntityCard(
                entityId,
                cardModel.Name,
                cardAspects,
                cardAntiAspects,
                cardModel.Description);

            return ent;
        }

        private IAspect CreateAspect(KeyValuePair<string, int> aspect) 
            => _aspectFactory.Create(aspect.Key, aspect.Value);
    }
}