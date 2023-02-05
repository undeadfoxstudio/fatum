using System.Collections.Generic;

namespace TableMode
{
    public interface IAspectTrigger
    {
        public string Entity { get; }
        public string Action { get; }
        public string ExpiringAspect { get; }
        public IEnumerable<string> Aspects { get; }
        public int RuleWeight { get; }
    }
}