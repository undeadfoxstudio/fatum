using System.Collections.Generic;

namespace TableMode
{
    public interface IHoverEntityCardsController
    {
        IEnumerable<IEntityCardView> GetHoveredCard();
    }
    
    public interface IHoverActionCardsController
    {
        IEnumerable<IActionCardView> GetHoveredCard();
    }
}