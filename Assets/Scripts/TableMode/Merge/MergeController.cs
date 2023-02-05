using System;
using System.Collections.Generic;
using System.Linq;

namespace TableMode
{
    public class MergeController : IMergeController, IDisposable
    {
        private readonly IMoveActionCardsController _moveActionCardsController;
        private readonly ICardSpawner _cardSpawner;
        private readonly IHandController _handController;
        private readonly IRuleController _ruleController;
        private readonly ITableController _tableController;
        private readonly IAspectFactory _aspectFactory;
        private readonly IMergeStorage _mergeStorage;
        private readonly IInputController _inputController;

        public MergeController(
            IMoveActionCardsController moveActionCardsController,
            ICardSpawner cardSpawner,
            IHandController handController,
            IRuleController ruleController,
            ITableController tableController,
            IAspectFactory aspectFactory,
            IMergeStorage mergeStorage)
        {
            _moveActionCardsController = moveActionCardsController;
            _cardSpawner = cardSpawner;

            _handController = handController;
            _ruleController = ruleController;
            _tableController = tableController;
            _aspectFactory = aspectFactory;
            _mergeStorage = mergeStorage;
            _moveActionCardsController.OnActionCardDrop += MoveActionControllerOnActionCardDrop;
        }

        private void MoveActionControllerOnActionCardDrop()
        {
            var mergePair = _mergeStorage.GetMergePair();

            if (mergePair is { actionCardView: { }, entityCardView: { } })
            {
                if (CanMerge(mergePair.actionCardView, mergePair.entityCardView))
                    ProcessMerge(mergePair.actionCardView, mergePair.entityCardView);
                else
                    _handController.TakeCard(mergePair.actionCardView);
            }
            else
            {
                _handController.TakeCard(mergePair.actionCardView);
            }

            _mergeStorage.ClearActionCardView();
            _mergeStorage.ClearEntityCardView();
        }

        private bool CanMerge(ICardView actionCardView, ICardView entityCardView)
        {
            if (HasBlockingAspects(actionCardView.Aspects, entityCardView.AntiAspects)) return false;
            if (HasBlockingAspects(actionCardView.AntiAspects, entityCardView.Aspects)) return false;

            return SameAspects(actionCardView.Aspects, entityCardView.Aspects).Any();
        }

        private void ProcessMerge(IActionCardView actionCard, IEntityCardView entityCard)
        {
            var aspects = SameAspects(actionCard.Aspects, entityCard.Aspects)
                .Select(aspect => aspect.Id).ToList();

            foreach (var aspectId in aspects)
                entityCard.DisableAspect(aspectId);

            var mergeTrigger = new MergeTrigger(
                entityCard.Id,
                actionCard.Id,
                aspects,
                0);

            _handController.RemoveCard(actionCard);
            
            var result = _ruleController.GetResult(mergeTrigger);
            if (result == null) return;
    
            var currentTableSlot = _tableController.GetSlotPositionByEntityCardView(entityCard);

            if (result.AspectsToAdd.Any())
            {
                foreach (var addedAspect in result.AspectsToAdd)
                    entityCard.AddAspect(
                        _aspectFactory.Create(addedAspect.Key, addedAspect.Value)
                        );
            }

            if (result.AspectsToDelete.Any())
            {
                foreach (var aspectId in result.AspectsToDelete)
                    entityCard.RemoveAspect(aspectId);
            }

            if (!string.IsNullOrWhiteSpace(result.EntityCardIdToAdd))
                _cardSpawner.SpawnEntity(result.EntityCardIdToAdd, currentTableSlot);

            if (!string.IsNullOrWhiteSpace(result.ActionCardIdToAdd))
                _cardSpawner.SpawnActionCardById(result.ActionCardIdToAdd, currentTableSlot);

            if (result.EntitiesFromGroupToAdd.Value > 0)
                _cardSpawner.SpawnEntities(
                    result.EntitiesFromGroupToAdd.Key,
                    result.EntitiesFromGroupToAdd.Value,
                    currentTableSlot);

            if (result.IsEntityCardDestroyed)
                _tableController.RemoveCard(entityCard);
        }

        private bool HasBlockingAspects(IEnumerable<IAspect> firstAspects, IEnumerable<IAspect> secondAspects)
        {
            return firstAspects
                .Select(firstAspect => secondAspects.FirstOrDefault(s => s.Name == firstAspect.Name))
                .Any(sameAspect => sameAspect != null);
        }

        private IEnumerable<IAspect> SameAspects(IEnumerable<IAspect> firstAspects, IEnumerable<IAspect> secondAspects)
        {
            var firstActiveAspects = firstAspects.Where(a => a.IsActive);
            var secondActiveAspects = secondAspects.Where(a => a.IsActive);

            return firstActiveAspects
                .Select(entityCardAspect => secondActiveAspects.FirstOrDefault(a => a.Name == entityCardAspect.Name))
                .Where(merge => merge != null)
                .ToList();
        }

        public void Dispose()
        {
            _moveActionCardsController.OnActionCardDrop -= MoveActionControllerOnActionCardDrop;
        }
    }
}