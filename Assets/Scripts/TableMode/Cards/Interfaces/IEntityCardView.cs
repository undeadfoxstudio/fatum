using System;

namespace TableMode
{
    public interface IEntityCardView : ICardView
    {
        void UpdateGradient();
        event Action<IEntityCardView> OnDestroy;
        void SwitchModeTo(CardReady mode);
        void DisableAspect(string aspectId);
    }
}