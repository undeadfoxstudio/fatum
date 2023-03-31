using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace TableMode
{
    public class RuleController : IRuleController
    {
        private readonly IContentProvider _contentProvider;

        public RuleController(IContentProvider contentProvider)
        {
            _contentProvider = contentProvider;
        }

        public IMergeResult GetResult(IMergeTrigger mergeTrigger)
        {
            var results = GetRightResults(mergeTrigger);
            var bestResult = results.FirstOrDefault();

            foreach (var result in results)
                if (result.Value > bestResult.Value) bestResult = result;

            return bestResult.Key;
        }

        private IDictionary<IMergeResult, int> GetRightResults(IMergeTrigger mergeTrigger)
        {
            Debug.Log(JsonConvert.SerializeObject(mergeTrigger));
            
            var results = new Dictionary<IMergeResult, int>();
            var mergeRuleModels = _contentProvider.MergeRuleModels();
           
            foreach (var mergeRuleModel in mergeRuleModels)
            {
                
                Debug.Log("111");
                if (mergeRuleModel.Trigger.Action != mergeTrigger.Action &&
                    !string.IsNullOrEmpty(mergeRuleModel.Trigger.Action))
                    continue;
                Debug.Log("222");
                Debug.Log(JsonConvert.SerializeObject(mergeRuleModel));
                if (mergeRuleModel.Trigger.Entity != mergeTrigger.Entity &&
                    !string.IsNullOrEmpty(mergeRuleModel.Trigger.Entity))
                    continue;
                Debug.Log("333");
                var weight = 0;

                if (mergeTrigger.Action == mergeRuleModel.Trigger.Action) weight += 10;
                if (mergeTrigger.Entity == mergeRuleModel.Trigger.Entity) weight += 10;

                weight += mergeTrigger.Aspects
                    .Select(m => mergeRuleModel.Trigger.Aspects.FirstOrDefault(a => a == m))
                    .Count(aspect => aspect != null);

                if (weight > 0) results.Add(mergeRuleModel.AspectResult, weight);
            }
            
            return results;
        }
    }
}