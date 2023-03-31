using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace TableMode
{
    public class MergeController : IMergeController, IDisposable
    {
        private readonly IMoveActionCardsController _moveActionCardsController;
        private readonly ICardSpawner _cardSpawner;
        private readonly IHandController _handController;
        private readonly IRuleController _ruleController;
        private readonly ITableController _tableController;
        private readonly IAspectViewFactory _aspectViewFactory;
        private readonly IMergeStorage _mergeStorage;
        private readonly IUIController _uiController;
        private readonly IInputController _inputController;

        public MergeController(
            IMoveActionCardsController moveActionCardsController,
            ICardSpawner cardSpawner,
            IHandController handController,
            IRuleController ruleController,
            ITableController tableController,
            IAspectViewFactory aspectViewFactory,
            IMergeStorage mergeStorage,
            IUIController uiController)
        {
            _moveActionCardsController = moveActionCardsController;
            _cardSpawner = cardSpawner;

            _handController = handController;
            _ruleController = ruleController;
            _tableController = tableController;
            _aspectViewFactory = aspectViewFactory;
            _mergeStorage = mergeStorage;
            _uiController = uiController;
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
            
            entityCard.UpdateGradient();

            var mergeTrigger = new MergeTrigger(
                entityCard.Id,
                actionCard.Id,
                aspects,
                0);

            Debug.Log(JsonConvert.SerializeObject(mergeTrigger));
            
            _handController.RemoveCard(actionCard);
            
            var mergeResult = _ruleController.GetResult(mergeTrigger);
            
            

            if (mergeResult == null) return;
    
            _uiController.PushLogMerge(actionCard.Name, entityCard.Name, mergeResult.Log);            
            
            var currentTableSlot = _tableController.GetSlotPositionByEntityCardView(entityCard);

            if (mergeResult.AspectsToAdd.Any())
            {
                foreach (var addedAspect in mergeResult.AspectsToAdd)
                    entityCard.AddAspect(_aspectViewFactory.CreateAspect(addedAspect.Key, addedAspect.Value));
            }

            if (mergeResult.AspectsToDelete.Any())
            {
                foreach (var aspectId in mergeResult.AspectsToDelete)
                    entityCard.RemoveAspect(aspectId);
            }

            if (!string.IsNullOrWhiteSpace(mergeResult.EntityCardIdToAdd))
                _cardSpawner.SpawnEntity(mergeResult.EntityCardIdToAdd, currentTableSlot);

            if (!string.IsNullOrWhiteSpace(mergeResult.ActionCardIdToAdd))
                _cardSpawner.SpawnActionCardById(mergeResult.ActionCardIdToAdd, currentTableSlot);

            
            
            if (mergeResult.EntitiesFromGroupToAdd.Value > 0)
            {
                
                _cardSpawner.SpawnEntities(
                    mergeResult.EntitiesFromGroupToAdd.Key,
                    mergeResult.EntitiesFromGroupToAdd.Value,
                    currentTableSlot);
            }

            if (mergeResult.IsEntityCardDestroyed)
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