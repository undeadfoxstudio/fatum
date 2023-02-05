using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;

namespace TableMode
{
    public class StepController : IStepController
    {
        private readonly IHandController _handController;
        private readonly ICardSpawner _cardSpawner;
        private readonly ITableController _tableController;
        private readonly IAspectController _aspectController;
        private readonly IAspectFactory _aspectFactory;
        private readonly StepController _stepController;

        public StepController(
            IHandController handController, 
            IUIBehavior iuiBehavior,
            ICardSpawner cardSpawner,
            ITableController tableController,
            IAspectController aspectController,
            IAspectFactory aspectFactory)
        {
            _handController = handController;
            _cardSpawner = cardSpawner;
            _tableController = tableController;
            _aspectController = aspectController;
            _aspectFactory = aspectFactory;

            iuiBehavior.OnNextStep += ControlBehaviorOnNextStep;
        }

        private void ControlBehaviorOnNextStep()
        {
            _handController
                .GetExpiredActionCards()
                .ToList()
                .ForEach(ProcessActionCardExpiredAspects);

            _tableController
                .GetExpiredEntityCards()
                .ToList()
                .ForEach(ProcessEntityCardExpiredAspects);

            if (!_handController.IsFull)
                _cardSpawner.SpawnActionCardDefault();

            _handController.NextStep();
            _tableController.NextStep();
        }

        private void ProcessEntityCardExpiredAspects(ICardView entityCardView)
        {
            var allEntityCardAspectResults = new List<IAspectResult>();
            var isCardDeleted = false;
            var removedAspects = new List<string>();

            //collect all processed results
            foreach (var aspect in entityCardView.Aspects.Where(a => a.Count == 1))
            {
                removedAspects.Add(aspect.Id);

                var currentAspectResults = _aspectController.GetEntityCardAspectResult(
                    aspect,
                    entityCardView.Aspects.Where(a => a.Id != aspect.Id).ToList(),
                    entityCardView.Id);

                if (!currentAspectResults.Any()) continue;

                allEntityCardAspectResults.AddRange(currentAspectResults);

                if (currentAspectResults.Any(r => r.IsEntityCardDestroyed))
                    isCardDeleted = true;
            }

            //remove aspects
            foreach (var entityCardAspectResult in allEntityCardAspectResults)
                foreach (var aspectToDelete in entityCardAspectResult.AspectsToDelete)
                    entityCardView.RemoveAspect(aspectToDelete);

            //add aspects
            var addingAspects = allEntityCardAspectResults
                .SelectMany(entityCardAspectResult => entityCardAspectResult.AspectsToAdd)
                .ToDictionary(aspectGroup => aspectGroup.Key, keyValuePair => keyValuePair.Value);

            foreach (var aspectGroup in addingAspects)
                entityCardView.AddAspect(_aspectFactory.Create(aspectGroup.Key, aspectGroup.Value));

            //should be before spawning new entities
            var cardPosition = _tableController.GetSlotPositionByEntityCardView((IEntityCardView)entityCardView);
            Debug.Log(cardPosition);
            
            if (isCardDeleted) _tableController.RemoveCard((IEntityCardView)entityCardView);
            
            //add new entity
            foreach (var entityCardAspectResult in allEntityCardAspectResults)
            {
                if (!entityCardAspectResult.EntityCardIdToAdd.IsEmpty())
                    _cardSpawner.SpawnEntity(entityCardAspectResult.EntityCardIdToAdd, cardPosition);
                
                _cardSpawner.SpawnEntities(
                    entityCardAspectResult.EntitiesFromGroupToAdd.Key,
                    entityCardAspectResult.EntitiesFromGroupToAdd.Value, 
                    cardPosition);
            }

            //add new actions
            foreach (var allActionCardAspectResult in allEntityCardAspectResults)
            {
                if (!allActionCardAspectResult.ActionCardIdToAdd.IsEmpty())
                    _cardSpawner.SpawnActionCardById(allActionCardAspectResult.ActionCardIdToAdd, Vector2Int.zero);

                _cardSpawner.SpawnActionCardFromGroup(
                    allActionCardAspectResult.ActionsFromGroupToAdd.Key,
                    allActionCardAspectResult.ActionsFromGroupToAdd.Value);
            }
        }

        private void ProcessActionCardExpiredAspects(ICardView actionCardView)
        {
            var allActionCardAspectResults = new List<IAspectResult>();
            var isCardDeleted = false;
            var removedAspects = new List<string>();

            //collect all processed results
            foreach (var aspect in actionCardView.Aspects.Where(a => a.Count == 1))
            {
                removedAspects.Add(aspect.Id);

                var currentAspectResults = _aspectController.GetActionCardAspectResult(
                    aspect,
                    actionCardView.Aspects.Where(a => a.Id != aspect.Id).ToList(),
                    actionCardView.Id);

                if (!currentAspectResults.Any()) continue;

                allActionCardAspectResults.AddRange(currentAspectResults);

                if (currentAspectResults.Any(r => r.IsActionCardDestroyed))
                    isCardDeleted = true;
            }

            //remove aspects
            foreach (var actionCardAspectResult in allActionCardAspectResults)
                foreach (var aspectToDelete in actionCardAspectResult.AspectsToDelete)
                    actionCardView.RemoveAspect(aspectToDelete);

            //add aspects
            var addingAspects = allActionCardAspectResults
                .SelectMany(actionCardAspectResult => actionCardAspectResult.AspectsToAdd)
                .ToDictionary(aspectGroup => aspectGroup.Key, keyValuePair => keyValuePair.Value);

            foreach (var aspectGroup in addingAspects)
                actionCardView.AddAspect(_aspectFactory.Create(aspectGroup.Key, aspectGroup.Value));

            //add new entity
            foreach (var actionCardAspectResult in allActionCardAspectResults)
            {
                if (!actionCardAspectResult.EntityCardIdToAdd.IsEmpty())
                    _cardSpawner.SpawnEntity(actionCardAspectResult.EntityCardIdToAdd);
                
                _cardSpawner.SpawnEntities(
                    actionCardAspectResult.EntitiesFromGroupToAdd.Key,
                    actionCardAspectResult.EntitiesFromGroupToAdd.Value);
            }

            //add new actions
            foreach (var allActionCardAspectResult in allActionCardAspectResults)
            {
                if (!allActionCardAspectResult.ActionCardIdToAdd.IsEmpty())
                    _cardSpawner.SpawnActionCardById(allActionCardAspectResult.ActionCardIdToAdd, Vector2Int.zero);

                _cardSpawner.SpawnActionCardFromGroup(
                    allActionCardAspectResult.ActionsFromGroupToAdd.Key,
                    allActionCardAspectResult.ActionsFromGroupToAdd.Value);
            }

            //always should be last
            if (isCardDeleted) _handController.RemoveCard((IActionCardView)actionCardView);
        }
    }
}