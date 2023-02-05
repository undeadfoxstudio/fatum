namespace TableMode
{
    public struct MergeRuleModel : IMergeRuleModel
    {
        public IMergeTrigger Trigger { get; }
        public IMergeResult AspectResult { get; }

        public MergeRuleModel(
            IMergeTrigger trigger, 
            IMergeResult aspectResult)
        {
            Trigger = trigger;
            AspectResult = aspectResult;
        }
    }
}