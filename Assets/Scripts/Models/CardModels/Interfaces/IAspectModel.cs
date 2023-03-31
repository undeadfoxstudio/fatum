namespace TableMode
{
    public interface IAspectModel
    {
        public string Id { get; }
        public string Asset { get; }
        public int Order { get; }
        public string Group { get; }
        public string Color { get; }
        public string Name { get; }
        public string Description { get; }
        public IAspectResult AspectResult { get; }
    }
}