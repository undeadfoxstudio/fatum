namespace TableMode
{
    public interface ISlotController
    {
        void CatchCard(ICardView entityCard);
        void NextStep();
    }
}