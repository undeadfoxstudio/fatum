using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TableMode
{
    public class HandController : IHandController
    {
        private readonly HandConfig _handConfig;
        private readonly IList<IActionCardView> _handCards = new List<IActionCardView>();

        public bool IsFull => _handCards.Count >= _handConfig.CardCount;

        public HandController(HandConfig handConfig)
        {
            _handConfig = handConfig;
        }

        private List<Vector3> GetPositions(int count)
        {
            var slots = new List<Vector3>();
            var length = _handConfig.HandCenterPosition.x - (count - 1) * _handConfig.SlotSize / 2;

            for (var i = 0; i < count; i++)
            {
                slots.Add(new Vector3(
                    length + i * _handConfig.SlotSize,
                    _handConfig.HandCenterPosition.y,
                    _handConfig.HandCenterPosition.z));
            }

            return slots;
        }

        public void ArrangeCard()
        {
            var positions = GetPositions(_handCards.Count);
            var i = 0;

            foreach (var actionCardView in _handCards)
            {
                actionCardView.Move(positions[i++], true);
                actionCardView.Rotate(0f);
            }
        }

        public void CatchCard(ICardView card)
        {
            if (!_handCards.Contains((IActionCardView)card)) return;
            
            _handCards.Remove((IActionCardView)card);
            ArrangeCard();
        }

        public void NextStep()
        {
            foreach (var actionCardView in _handCards)
            {
                foreach (var aspect in actionCardView.Aspects)
                    aspect.Update();

                foreach (var antiAspect in actionCardView.AntiAspects)
                    antiAspect.Update();

                actionCardView.NextStep();
            }
        }

        public void TakeCard(IActionCardView card)
        {
            _handCards.Add(card);

            ArrangeCard();
        }

        public void RemoveCard(IActionCardView card)
        {
            _handCards.Remove(card);
            card.Destroy();

            ArrangeCard();
        }

        public IEnumerable<IActionCardView> GetExpiredActionCards() =>
            _handCards.Where(c => 
                c.Aspects.Any(a => a.Count == 1) ||
                c.AntiAspects.Any(a => a.Count == 1));
    }
}