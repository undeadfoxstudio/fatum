using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TableMode
{
    public class TableController : ITableController
    {
        private readonly ITableProvider _tableProvider;
        private readonly Dictionary<Vector2Int, IEntityCardView> _cardPositions = new ();

        private Dictionary<Vector2Int, Vector3> FreeSlotPositions
        {
            get
            {
                var slots = _tableProvider.Positions;
                var freeSlots = slots
                    .Where(s => !_cardPositions.Keys.Contains(s.Key))
                    .ToDictionary(s => s.Key, s => s.Value);

                return freeSlots;
            }
        }

        public TableController(ITableProvider tableProvider)
        {
            _tableProvider = tableProvider;
        }

        public void CatchCard(ICardView catchedEntityCard)
        {
            var card = _cardPositions
                .FirstOrDefault(c => c.Value == catchedEntityCard);

            if (card.Value != null) _cardPositions.Remove(card.Key);
        }

        public void NextStep()
        {
            foreach (var cardPositionsValue in _cardPositions.Values)
                cardPositionsValue.NextStep();

            var atRemove = _cardPositions.Values
                .Where(c => !c.Aspects.Any())
                .ToArray();

            foreach (var t in atRemove)
                RemoveCard(t);
        }

        public void Clear()
        {
            foreach (var entityCardView in _cardPositions)
                entityCardView.Value.Destroy();

            _cardPositions.Clear();
        }

        public Vector3 TakeCard(IEntityCardView entityCardView)
        {
            var newSlotPosition = GetBestNearbySlot(entityCardView.Position);
            var newTransformPosition = _tableProvider.GetSlotPosition(newSlotPosition);

            _cardPositions.Add(newSlotPosition, entityCardView);

            return newTransformPosition;
        }

        public void RemoveCard(IEntityCardView entityCard)
        {
            var entityKeyPair = _cardPositions.FirstOrDefault(c => c.Value == entityCard);

            _cardPositions.Remove(entityKeyPair.Key);
            entityCard.Destroy();
        }

        public void SwitchCardState(CardReady mode)
        {
            foreach (var cardPositionsValue in _cardPositions.Values)
                cardPositionsValue.SwitchModeTo(mode);
        }

        public IEnumerable<IEntityCardView> GetExpiredEntityCards()
        {
            return _cardPositions.Values.Where(c => 
                c.Aspects.Any(a => a.Count == 1) ||
                c.AntiAspects.Any(a => a.Count == 1));
        }

        public IEnumerable<string> GetTableCardIds() => _cardPositions.Values.Select(c => c.Id).Distinct();

        public Vector2Int GetSlotPositionByEntityCardView(IEntityCardView entityCardView) =>
            _cardPositions.FirstOrDefault(c => c.Value == entityCardView).Key;

        public Vector2Int GetRandomSlotPosition()
        {
            var randomSlot = _tableProvider.Positions.ElementAt(
                Random.Range(0, _tableProvider.Positions.Count));

            return GetBestNearbySlot(randomSlot.Value);
        }

        public Vector3 GetSlotPosition(Vector2Int position) =>
            _tableProvider.GetSlotPosition(position);

        public Vector3 SetCardNearby(Vector2Int position, IEntityCardView entityCardView)
        {
            if (FreeSlotPositions.Keys.Contains(position))
            {
                _cardPositions.Add(position, entityCardView);

                return _tableProvider.Positions[position];
            }

            var currentSlotPosition = _tableProvider.Positions[position];
            var nearbySlot = GetBestNearbySlot(currentSlotPosition);
            
            _cardPositions.Add(nearbySlot, entityCardView);

            return _tableProvider.Positions[nearbySlot];
        }

        private Vector2Int GetBestNearbySlot(Vector3 currentPosition)
        {
            if (FreeSlotPositions.Count == 0)
                throw new Exception("Not found empty slot.");

            
            var nearbySlotLength = GetVectors3Length(currentPosition, FreeSlotPositions.First().Value);
            var nearbySlotPosition = FreeSlotPositions.First().Key;
            
            foreach (var freeSlotPosition in FreeSlotPositions)
            {
                var currentSlotLength = GetVectors3Length(currentPosition, freeSlotPosition.Value);
                
                if (currentSlotLength < nearbySlotLength)
                {
                    nearbySlotLength = currentSlotLength;
                    nearbySlotPosition = freeSlotPosition.Key;
                }
            }

            return nearbySlotPosition;
        }

        private double GetVectors3Length(Vector3 firstVector3, Vector3 secondVector3)
        {
            var firstVector2 = new Vector2(firstVector3.x, firstVector3.z);
            var secondVector2 = new Vector2(secondVector3.x, secondVector3.z);

            return Vector2.Distance(firstVector2, secondVector2);
        }
    }
}