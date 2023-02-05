namespace TableMode
{
    public interface IAspectRuleModel
    {
        public IAspectTrigger AspectTrigger { get; }
        public IAspectResult AspectResult { get; }
    }
}