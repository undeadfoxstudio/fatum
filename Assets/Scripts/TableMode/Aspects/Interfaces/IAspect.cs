namespace TableMode
{
    public interface IAspect
    {
        bool IsActive { get; set; }
        string Id { get; }
        string Name { get; }
        int Count { get; set; }
        void Update();
    }
}