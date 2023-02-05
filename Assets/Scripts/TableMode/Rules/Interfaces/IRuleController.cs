namespace TableMode
{
    public interface IRuleController
    {
        IMergeResult GetResult(IMergeTrigger mergeTrigger);
    }
}