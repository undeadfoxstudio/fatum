namespace TableMode
{
    public interface IMergeRuleModel
    {
        public IMergeTrigger Trigger { get; }
        public IMergeResult AspectResult { get; }
    }
}