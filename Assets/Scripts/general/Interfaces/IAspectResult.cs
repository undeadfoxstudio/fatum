namespace TableMode
{
    public interface IAspectResult : IResult
    {
        bool IsEntityCardDestroyed { get; } 
        bool IsActionCardDestroyed { get; }
    }
}