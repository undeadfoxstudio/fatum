using System;
using System.Collections.Generic;

namespace TableMode
{
    public interface IHoverEntityCardsController
    {
        event Action<IEntityCardView> OnActionCardViewHover;
        event Action OnActionCardViewLeave;
        IEnumerable<IEntityCardView> GetHoveredCard();
    }
    
    public interface IHoverActionCardsController
    {
        event Action<IActionCardView> OnActionCardViewHover;
        event Action OnActionCardViewLeave;
        IEnumerable<IActionCardView> GetHoveredCard();
    }
}