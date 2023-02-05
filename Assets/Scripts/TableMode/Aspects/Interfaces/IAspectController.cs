using System.Collections.Generic;

namespace TableMode
{
    public interface IAspectController
    {
        public IList<IAspectResult> GetActionCardAspectResult(IAspect aspect, IList<IAspect> otherAspects, string actionId);
        public IList<IAspectResult> GetEntityCardAspectResult(IAspect aspect, IList<IAspect> otherAspects, string entityId);
    }
}