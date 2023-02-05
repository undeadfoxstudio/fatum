namespace TableMode
{
    public interface IMergeResult : IResult
    {
        bool IsEntityCardDestroyed { get; }
    }
}