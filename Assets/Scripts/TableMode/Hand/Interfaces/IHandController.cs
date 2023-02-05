using System.Collections.Generic;

namespace TableMode
{
    public interface IHandController : ISlotController
    {
        bool IsFull { get; }
        void ArrangeCard();
        void TakeCard(IActionCardView card);
        void RemoveCard(IActionCardView card);
        IEnumerable<IActionCardView> GetExpiredActionCards();
    }
}