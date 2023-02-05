namespace TableMode
{
    public interface IAspectFactory
    {
        IAspect Create(string aspectId, int count);
    }
}