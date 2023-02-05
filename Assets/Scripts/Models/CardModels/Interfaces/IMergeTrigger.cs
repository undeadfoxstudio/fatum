using System.Collections.Generic;

namespace TableMode
{
    public interface IMergeTrigger
    {
        public string Entity { get; }
        public string Action { get; }
        public IEnumerable<string> Aspects { get; }
        public int RuleWeight { get; }
    }
}