namespace TableMode
{
    public struct AspectModel : IAspectModel
    {
        public string Id { get; }
        public string Asset { get; }
        public int Order { get; }
        public string Group { get; }
        public string Color { get; }
        public string Name { get; }
        public string Description { get; }
        public IAspectResult AspectResult { get; }

        public AspectModel(
            string id,
            string asset, 
            int order,
            string group,
            string name,
            string description, 
            IAspectResult aspectResult, 
            string color)
        {
            Id = id;
            Order = order;
            Group = group;
            Name = name;
            Description = description;
            AspectResult = aspectResult;
            Color = color;
            Asset = asset;
        }
    }
}