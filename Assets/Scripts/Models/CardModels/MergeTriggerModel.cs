using System.Collections.Generic;

namespace TableMode
{
    public struct MergeTrigger : IMergeTrigger
    {
        public string Entity { get; }
        public string Action { get; }
        public IEnumerable<string> Aspects { get; }
        public int RuleWeight { get; }

        public MergeTrigger(
            string entity, 
            string action, 
            IEnumerable<string> aspects, 
            int ruleWeight)
        {
            Entity = entity;
            Action = action;
            Aspects = aspects;
            RuleWeight = ruleWeight;
        }
    }
}