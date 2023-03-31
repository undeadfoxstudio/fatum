using System.Collections.Generic;

namespace TableMode
{
    public interface IAspectRuleProvider
    {
        public IList<IAspectResult> GetActionCardAspectResults(IAspect aspect, IList<IAspect> otherAspects, string actionId);
        public IList<IAspectResult> GetEntityCardAspectResults(IAspect aspect, IList<IAspect> otherAspects, string entityId);
    }
}