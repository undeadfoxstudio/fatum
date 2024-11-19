using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ModestTree;
using UnityEngine;

namespace TableMode
{
    public class StepController : IStepController
    {
        private readonly DateTime startDate = new (1970, 3, 2);
        
        private readonly IUIController _iUIController;
        private readonly IHandController _handController;
        private readonly ICardSpawner _cardSpawner;
        private readonly ITableController _tableController;
        private readonly IAspectRuleProvider _aspectRuleProvider;
        private readonly IAspectViewFactory _aspectViewFactory;

        public StepController(
            IUIController iUIController,
            IHandController handController,
            ICardSpawner cardSpawner,
            ITableController tableController,
            IAspectRuleProvider aspectRuleProvider,
            IAspectViewFactory aspectViewFactory)
        {
            _iUIController = iUIController;
            _handController = handController;
            _cardSpawner = cardSpawner;
            _tableController = tableController;
            _aspectRuleProvider = aspectRuleProvider;
            _aspectViewFactory = aspectViewFactory;

            _iUIController.OnNextStep += ControlBehaviorOnNextStep;
        }

        private void ControlBehaviorOnNextStep(int step)
        {
            var newDate = startDate
                .AddDays(step)
                .ToString("dddd, dd MMMM", CultureInfo.CreateSpecificCulture("en-US"))
                .FirstCharToUpper();
            
            _iUIController.SetNextStepLog(newDate);
            _iUIController.PushLog(" ------ " + newDate + " ------ \n");

            _handController
                .GetExpiredActionCards()
                .ToList()
                .ForEach(ProcessActionCardExpiredAspects);

            _tableController
                .GetExpiredEntityCards()
                .ToList()
                .ForEach(ProcessEntityCardExpiredAspects);

            var newCardNeed = _handController.CardNeeded;

            for (var i = 0; i < newCardNeed; i++)
            _cardSpawner.SpawnActionCardDefault();
           // _cardSpawner.SpawnActionCardDefault();
            
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

                _iUIController.PushLog(
                    "Aspect  <color=blue>" +
                    aspect.Name + "</color> (<color=red>" + 
                    entityCardView.Name + 
                    "</color>): has expired. \n");

                var currentAspectResults = _aspectRuleProvider.GetEntityCardAspectResults(
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

            //remove antiAspects
            foreach (var aspect in entityCardView.AntiAspects.Where(a => a.Count == 1).ToList())
                entityCardView.RemoveAntiAspect(aspect.Id);
            
            //remove expired aspects
            foreach (var removedAspect in removedAspects)
                entityCardView.RemoveAspect(removedAspect);
            
            //add aspects
            var addingAspects = allEntityCardAspectResults
                .SelectMany(entityCardAspectResult => entityCardAspectResult.AspectsToAdd)
                .ToDictionary(aspectGroup => aspectGroup.Key, keyValuePair => keyValuePair.Value);

            foreach (var aspectGroup in addingAspects)
                entityCardView.AddAspect(_aspectViewFactory.CreateAspect(aspectGroup.Key, aspectGroup.Value + 1));

            //should be BEFORE spawning new entities
            var cardPosition = _tableController.GetSlotPositionByEntityCardView((IEntityCardView)entityCardView);
            if (isCardDeleted)
            {
                _iUIController.PushLog(
                    "<color=red>" +
                    entityCardView.Name + "</color> is disappearing." +
                    "\n");

                _tableController.RemoveCard((IEntityCardView)entityCardView);
            }

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
                _iUIController.PushLog(
                    "Aspect  <color=blue>" +
                    aspect.Name + "</color> (<color=red>" + 
                    actionCardView.Name + 
                    "</color>): has expired. \n");

                removedAspects.Add(aspect.Id);

                var currentAspectResults = _aspectRuleProvider.GetActionCardAspectResults(
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
                actionCardView.AddAspect(_aspectViewFactory.CreateAspect(aspectGroup.Key, aspectGroup.Value));

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
            if (isCardDeleted)
            {
                _handController.RemoveCard((IActionCardView)actionCardView);
            }
        }
    }
    
    public static class Extensions
    {
        public static string FirstCharToUpper(this string str)
        {
            if (String.IsNullOrEmpty(str)) {
                throw new ArgumentException("Input string is empty!");
            }
 
            return str.First().ToString().ToUpper() + string.Concat(str.Skip(1));
        }
    }
}