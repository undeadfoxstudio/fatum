﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TableMode
{
    public class CardSpawner : ICardSpawner
    {
        private readonly ICardViewFactory _cardViewFactory;
        private readonly IContentProvider _contentProvider;
        private readonly ITableController _tableController;
        private readonly IHandController _handController;

        private int i = 0;

        private readonly List<string> _promoOrderActionCards = new List<string>()
        {
            "use_first",
            "focus_first",
            "behold_first",
            "talk_first",
            "behold_first",
            "behold_first",
            "use_first",
            "focus_first",
            "use_first",
            "focus_first",
            "talk_first",
            "talk_first",
            "focus_first",
            "focus_first",
            "focus_first",
        };
            
        private readonly List<string> _firstCardIds = new List<string>
        {
            "behold_first", 
            "use_first", 
            "focus_first", 
            "talk_first"
        };
        
        public CardSpawner(ICardViewFactory cardViewFactory,
            IContentProvider contentProvider,
            ITableController tableController,
            IHandController handController)
        {
            _cardViewFactory = cardViewFactory;
            _contentProvider = contentProvider;
            _tableController = tableController;
            _handController = handController;
        }
        
        public void SpawnActionCardById(string cardId, Vector2Int position)
        {
            var card = _cardViewFactory.CreateActionCard(cardId);

            card.MoveImmediately(_tableController.GetSlotPosition(position), true);

            _handController.TakeCard(card);
        }

        public void SpawnActionCardFromGroup(string groupId, int count)
        {
            var actionCardIds = _contentProvider.GetActionIdsByGroup(groupId);

            for (var i = 0; i < count; i++)
            {
                var actionCardId = actionCardIds.ElementAt(Random.Range(0, actionCardIds.Count));
                var card = _cardViewFactory.CreateActionCard(actionCardId);

                card.MoveImmediately(_tableController.GetSlotPosition(Vector2Int.zero), true);

                _handController.TakeCard(card);
            }
        }

        public void SpawnActionCardDefault()
        {
            var randomId = _firstCardIds[Random.Range(0, _firstCardIds.Count)];
            
            var card = _cardViewFactory.CreateActionCard(_promoOrderActionCards[i]);
            
            i++;
            if (i >= _promoOrderActionCards.Count) i = 0;
            
            card.MoveImmediately(_tableController.GetSlotPosition(Vector2Int.zero), true);
            
            _handController.TakeCard(card);
            
            /*switch (i)
            {
                case 0:
                {
                    var randomId = _firstCardIds[Random.Range(0, _firstCardIds.Count)];
                    var card = _cardViewFactory.CreateActionCard("concentration");

                    card.MoveImmediately(_tableController.GetSlotPosition(Vector2Int.zero), true);
                    _handController.TakeCard(card);

                    card = _cardViewFactory.CreateActionCard("inspiration");

                    card.MoveImmediately(_tableController.GetSlotPosition(Vector2Int.zero), true);

                    _handController.TakeCard(card);
                    break;
                }
                case 1:
                {
                    var randomId = _firstCardIds[Random.Range(0, _firstCardIds.Count)];
                    var card = _cardViewFactory.CreateActionCard("destruction");

                    card.MoveImmediately(_tableController.GetSlotPosition(Vector2Int.zero), true);
                    _handController.TakeCard(card);

                    card = _cardViewFactory.CreateActionCard("curiosity");

                    card.MoveImmediately(_tableController.GetSlotPosition(Vector2Int.zero), true);

                    _handController.TakeCard(card);
                    
                    card = _cardViewFactory.CreateActionCard("loneliness");

                    card.MoveImmediately(_tableController.GetSlotPosition(Vector2Int.zero), true);

                    _handController.TakeCard(card);
                    break;
                }
            }

            i++;*/
        }

        public void SpawnEntity(string entitiesId, Vector2Int? position = null)
        {
            var slotPosition = position ?? _tableController.GetRandomSlotPosition();

            if (string.IsNullOrEmpty(entitiesId)) return;

            var card = _cardViewFactory.CreateEntityCard(entitiesId);
            var newPosition = _tableController.SetCardNearby(slotPosition, card);

            card.MoveImmediately(_tableController.GetSlotPosition(slotPosition));
            card.Move(newPosition);
        }

        public void SpawnEntities(string groupName, int count, Vector2Int? position = null)
        {
            var isRandom = position == null;
            var slotPosition = position ?? _tableController.GetRandomSlotPosition();
            
            if (string.IsNullOrEmpty(groupName)) return;
                
            var cardIds = _contentProvider
                .GetEntityIdsFromGroup(groupName)
                .ToList();

            if (cardIds.Count == 0) return;

            for (var i = 0; i < count; i++)
                SpawnEntity(
                    cardIds.ElementAt(Random.Range(0, cardIds.Count)),
                    isRandom ? _tableController.GetRandomSlotPosition() : slotPosition);
        }
    }
}