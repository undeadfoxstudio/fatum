using System;

namespace TableMode
{
    public interface ICardViewFactory
    {
        event Action<IEntityCardView> OnCreateEntityCard;
        event Action<IActionCardView> OnCreateActionCard;
        IEntityCardView CreateEntityCard(string entityId);
        IActionCardView CreateActionCard(string actionId);
    }
}