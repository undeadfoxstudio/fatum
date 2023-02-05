using System.Collections.Generic;
using UnityEngine;

namespace TableMode
{
    public interface ITableController : ISlotController
    {
        Vector2Int GetSlotPositionByEntityCardView(IEntityCardView entityCardView);
        Vector2Int GetRandomSlotPosition();
        Vector3 GetSlotPosition(Vector2Int position);
        Vector3 SetCardNearby(Vector2Int position, IEntityCardView entityCardView);
        Vector3 TakeCard(IEntityCardView entityCard);
        void RemoveCard(IEntityCardView entityCard);
        void SwitchCardState(CardReady mode);
        IEnumerable<IEntityCardView> GetExpiredEntityCards();
    }
}