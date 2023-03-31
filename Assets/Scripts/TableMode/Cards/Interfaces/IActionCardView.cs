using System;

namespace TableMode
{
    public interface IActionCardView : ICardView
    {
        void Update();
        event Action<IActionCardView> OnDestroy;
    }
}