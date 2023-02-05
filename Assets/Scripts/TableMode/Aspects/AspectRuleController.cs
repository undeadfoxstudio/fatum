using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace TableMode
{
    public class AspectRuleController : IAspectController
    {
        private readonly IContentProvider _contentProvider;

        public AspectRuleController(IContentProvider contentProvider)
        {
            _contentProvider = contentProvider;
        }
        
        public IList<IAspectResult> GetActionCardAspectResult(IAspect aspect, IList<IAspect> otherAspects, string actionId)
        {
            return _contentProvider.AspectRuleModels()
                .Where(r => 
                    r.AspectTrigger.ExpiringAspect == aspect.Id && 
                    r.AspectTrigger.Action == actionId)
                .Select(r => r.AspectResult)
                .ToList();
        }

        public IList<IAspectResult> GetEntityCardAspectResult(IAspect aspect, IList<IAspect> otherAspects, string entityId)
        {
            return _contentProvider.AspectRuleModels()
                .Where(r => 
                    r.AspectTrigger.ExpiringAspect == aspect.Id)
                .Select(r => r.AspectResult)
                .ToList();
        }
    }
}