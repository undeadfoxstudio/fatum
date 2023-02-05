using System;

namespace TableMode
{
    public interface IActionCardView : ICardView
    {
        event Action<IActionCardView> OnDestroy;
    }
}