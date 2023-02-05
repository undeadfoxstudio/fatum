using System.Collections.Generic;

namespace TableMode
{
    public struct AspectTrigger : IAspectTrigger
    {
        public string Entity { get; }
        public string Action { get; }
        public string ExpiringAspect { get; }
        public IEnumerable<string> Aspects { get; }
        public int RuleWeight { get; }
        
        public AspectTrigger(string entity, string action, string expiringAspect, IEnumerable<string> aspects, int ruleWeight)
        {
            Entity = entity;
            Action = action;
            ExpiringAspect = expiringAspect;
            Aspects = aspects;
            RuleWeight = ruleWeight;
        }
    }
}