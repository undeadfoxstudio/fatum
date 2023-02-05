namespace TableMode
{
    public interface ICardFactory
    {
        IActionCard CreateActionCard(string actionId);
        IEntityCard CreateEntityCard(string entityId);
    }
}