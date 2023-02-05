namespace TableMode
{
    public interface IMergeStorage
    {
        (IActionCardView actionCardView, IEntityCardView entityCardView) GetMergePair();
        void SetActionCardView(IActionCardView actionCardView);
        void SetEntityCardView(IEntityCardView entityCardView);
        void ClearActionCardView();
        void ClearEntityCardView();
    }
}