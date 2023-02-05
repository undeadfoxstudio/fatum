namespace TableMode
{
    public struct AspectRuleModel : IAspectRuleModel
    {
        public IAspectTrigger AspectTrigger { get; }
        public IAspectResult AspectResult { get; }

        public AspectRuleModel(
            IAspectTrigger aspectTrigger,
            IAspectResult aspectResult)
        {
            AspectResult = aspectResult;
            AspectTrigger = aspectTrigger;
        }
    }
}