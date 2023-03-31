using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace TableMode
{
    public class AspectRuleProvider : IAspectRuleProvider
    {
        private readonly IContentProvider _contentProvider;

        public AspectRuleProvider(IContentProvider contentProvider)
        {
            _contentProvider = contentProvider;
        }
        
        public IList<IAspectResult> GetActionCardAspectResults(IAspect aspect, IList<IAspect> otherAspects, string actionId)
        {
            return _contentProvider.AspectRuleModels()
                .Where(r => 
                    (r.AspectTrigger.ExpiringAspect == aspect.Id && 
                    r.AspectTrigger.Action == actionId) || r.AspectTrigger.ExpiringAspect == aspect.Id)
                .Select(r => r.AspectResult)
                .ToList();
        }

        public IList<IAspectResult> GetEntityCardAspectResults(IAspect aspect, IList<IAspect> otherAspects, string entityId)
        {
            return _contentProvider.AspectRuleModels()
                .Where(r => 
                    r.AspectTrigger.ExpiringAspect == aspect.Id)
                .Where(r => r.AspectTrigger.Entity == entityId || r.AspectTrigger.Entity == "")
                .Select(r => r.AspectResult)
                .ToList();
        }
    }
}