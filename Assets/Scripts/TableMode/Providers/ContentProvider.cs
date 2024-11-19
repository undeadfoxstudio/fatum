using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Newtonsoft.Json;
using UnityEngine;

namespace TableMode
{
    public class ContentProvider : IContentProvider
    {
        private readonly ContentConfig _contentConfig;

        private List<IMergeRuleModel> _mergeRules = new ();
        private List<IAspectModel> _aspects = new ();
        private List<IActionCardModel> _actionCards = new ();
        private List<IEntityCardModel> _entityCards = new ();
        private List<IAspectRuleModel> _aspectRules = new ();

        private IEnumerable<IActionCardModel> ActionCards
        {
            get
            {
                if (_actionCards.Count == 0)
                    _actionCards = CSVReader.Read(_contentConfig.ActionCards.text)
                        .Select(GenerateActionCardContentByCSV)
                        .ToList();

                return _actionCards;
            }
        }

        private IEnumerable<IEntityCardModel> EntityCards
        {
            get
            {
                if (_entityCards.Count == 0)
                    _entityCards = CSVReader.Read(_contentConfig.EntityCards.text)
                        .Select(GenerateEntityCardContentByCSV)
                        .ToList();

                return _entityCards;
            }
        }

        private IEnumerable<IAspectRuleModel> AspectRules
        {
            get
            {
                if (_aspectRules.Count == 0)                
                    _aspectRules = CSVReader.Read(_contentConfig.AspectRules.text)
                        .Select(GenerateAspectRuleContentByCSV)
                        .ToList();
                
                return _aspectRules;
            }
        }

        private IEnumerable<IMergeRuleModel> MergeRules
        {
            get
            {
                if (_mergeRules.Count == 0)
                    _mergeRules = CSVReader.Read(_contentConfig.MergeRules.text)
                        .Select(GenerateMergeRuleContentByCSV)
                        .ToList();

                return _mergeRules;
            }
        }

        private IEnumerable<IAspectModel> Aspects
        {
            get
            {
                if (_aspects.Count == 0)
                    _aspects = CSVReader.Read(_contentConfig.Aspects.text)
                        .Select(GenerateAspectContentByCSV)
                        .ToList();

                return _aspects;
            }
        }

        public ContentProvider(ContentConfig contentConfig)
        {
            _contentConfig = contentConfig;
        }

        public IActionCardModel GetActionById(string id) =>
            ActionCards.FirstOrDefault(e => e.Id == id);

        public IList<string> GetActionGroups(string groupId) =>
            ActionCards.Select(a=> a.Group).Distinct().ToList();

        public IList<string> GetActionIdsByGroup(string groupId) =>
            ActionCards
                .Where(a => a.Group == groupId)
                .Select(a => a.Id)
                .ToList();
        
        public IEntityCardModel GetEntityById(string id) =>
            EntityCards.FirstOrDefault(e => e.Id == id);

        public IAspectModel GetAspectById(string id) => 
            Aspects.FirstOrDefault(a => a.Id == id);

        public IEnumerable<IMergeRuleModel> MergeRuleModels() => MergeRules;

        public IEnumerable<IAspectRuleModel> AspectRuleModels() => AspectRules;

        public IEnumerable<string> GetEntityIdsFromGroup(string groupId) => 
            EntityCards
                .Where(e => e.Group == groupId)
                .Select(e => e.Id)
                .ToList();

        public string GetRandomEntityIdFromGroup(string groupId, IList<string> exceptIds)
        {
            var ids = EntityCards
                .Where(e => e.Group == groupId)
                .Where(e => !exceptIds.Contains(e.Id))
                .Select(c => c.Id)
                .ToList();

            return ids.Count > 0 ? ids.ElementAt(Random.Range(0, ids.Count)) : string.Empty;
        }

        private IAspectRuleModel GenerateAspectRuleContentByCSV(Dictionary<string, object> data)
        {
            return new AspectRuleModel(
                GenerateAspectTriggerByCSV(data),
                GenerateAspectResultByCSV(data)
            );
        }

        private IMergeRuleModel GenerateMergeRuleContentByCSV(Dictionary<string, object> data)
        {
            return new MergeRuleModel(
                GenerateMergeTriggerByCSV(data),
                GenerateMergeResultByCSV(data)
            );
        }

        private IAspectTrigger GenerateAspectTriggerByCSV(Dictionary<string, object> data)
        {
            var aspects = new List<string>
            {
                data.GetString("Aspect1"),
                data.GetString("Aspect2"),
                data.GetString("Aspect3"),
                data.GetString("Aspect4"),
                data.GetString("Aspect5")
            }.Where(a => a != "");

            return new AspectTrigger(
                data.GetString("Entity"),
                data.GetString("Action"),
                data.GetString("ExpiringAspect"),
                aspects,
                data.GetInt("Weight")
            );
        }

        private IAspectResult GenerateAspectResultByCSV(Dictionary<string, object> data)
        {
            var aspectsToAdd = new Dictionary<string, int>();
            var antiAspectsToAdd = new Dictionary<string, int>();
            var aspectsToDelete = new List<string>();
            var antiAspectsToDelete = new List<string>();

            if (!data.GetString("AddAspect1").IsEmpty())
                aspectsToAdd.Add(data.GetString("AddAspect1"),data.GetInt("Aspect1Time"));
            if (!data.GetString("AddAspect2").IsEmpty())
                aspectsToAdd.Add(data.GetString("AddAspect2"),data.GetInt("Aspect2Time"));

            if (!data.GetString("AddAntiAspect1").IsEmpty())
                antiAspectsToAdd.Add(data.GetString("AddAntiAspect1Time"),data.GetInt("Aspect1Time"));
            if (!data.GetString("AddAntiAspect2").IsEmpty())
                antiAspectsToAdd.Add(data.GetString("AddAntiAspect2Time"),data.GetInt("Aspect2Time"));

            if (!data.GetString("DeleteAspect1").IsEmpty())
                aspectsToDelete.Add(data.GetString("DeleteAspect1"));
            if (!data.GetString("DeleteAspect2").IsEmpty())
                aspectsToDelete.Add(data.GetString("DeleteAspect2"));

            if (!data.GetString("DeleteAntiAspect1").IsEmpty())
                antiAspectsToDelete.Add(data.GetString("DeleteAntiAspect1"));
            if (!data.GetString("DeleteAntiAspect2").IsEmpty())
                antiAspectsToDelete.Add(data.GetString("DeleteAntiAspect2"));
            
            var IsEntityCardDestroyed = 
                data.GetString("DeleteEntity") != "" && data.GetString("DeleteEntity") != "0";

            var IsActionCardDestroyed =
                data.GetString("DeleteAction") != "" && data.GetString("DeleteAction") != "0";
            
            return new AspectResult(
                data.GetString("AddEntity"),
                data.GetString("AddAction"),
                new KeyValuePair<string, int>(
                    data.GetString("AddEntitiesFromGroup"),
                    data.GetInt("CountNewEntities")),
                aspectsToAdd,
                aspectsToDelete,
                IsEntityCardDestroyed,
                new KeyValuePair<string, int>(
                    data.GetString("AddActionsFromGroup"),
                    data.GetInt("CountNewActions")),
                antiAspectsToAdd,
                antiAspectsToDelete,
                IsActionCardDestroyed,
                data.GetString("Log")
                );
        }

        private IMergeTrigger GenerateMergeTriggerByCSV(Dictionary<string, object> data)
        {
            var aspects = new List<string>
            {
                data.GetString("Aspect1"),
                data.GetString("Aspect2"),
                data.GetString("Aspect3"),
                data.GetString("Aspect4"),
                data.GetString("Aspect5"),
                data.GetString("Aspect6")
            }.Where(a => a != "");

            return new MergeTrigger(
                data.GetString("EntityId"),
                data.GetString("ActionId"),
                aspects,
                data.GetInt("Weight"));
        }

        private IAspectModel GenerateAspectContentByCSV(Dictionary<string, object> data)
        {
            return new AspectModel
            (
                data.GetString("Id"),
                data.GetString("Asset"),
                data.GetInt("Order"),
                data.GetString("Group"),
                data.GetString("Name"),
                data.GetString("Description"),
                GenerateAspectResultByCSV(data),
                data.GetString("Color (RGB)")
            );
        }

        private IActionCardModel GenerateActionCardContentByCSV(Dictionary<string, object> data)
        {
            var aspects = new Dictionary<string, int>();

            if (!data.GetString("Aspect1").IsEmpty()) 
                aspects.Add(data.GetString("Aspect1"), data.GetInt("Aspect1Time"));
            if (!data.GetString("Aspect2").IsEmpty()) 
                aspects.Add(data.GetString("Aspect2"), data.GetInt("Aspect2Time"));
            if (!data.GetString("Aspect3").IsEmpty()) 
                aspects.Add(data.GetString("Aspect3"), data.GetInt("Aspect3Time"));
            if (!data.GetString("Aspect4").IsEmpty()) 
                aspects.Add(data.GetString("Aspect4"), data.GetInt("Aspect4Time"));
            if (!data.GetString("Aspect5").IsEmpty()) 
                aspects.Add(data.GetString("Aspect5"), data.GetInt("Aspect5Time")); 
            if (!data.GetString("Aspect6").IsEmpty()) 
                aspects.Add(data.GetString("Aspect6"), data.GetInt("Aspect6Time"));

            var antiAspects = new Dictionary<string, int>();

            if (!data.GetString("AntiAspect1").IsEmpty()) 
                antiAspects.Add(data.GetString("AntiAspect1"), data.GetInt("Aspect1Time"));
            if (!data.GetString("AntiAspect2").IsEmpty()) 
                antiAspects.Add(data.GetString("AntiAspect2"), data.GetInt("Aspect2Time"));
            if (!data.GetString("AntiAspect3").IsEmpty()) 
                antiAspects.Add(data.GetString("AntiAspect3"), data.GetInt("Aspect3Time"));
            if (!data.GetString("AntiAspect4").IsEmpty()) 
                antiAspects.Add(data.GetString("AntiAspect4"), data.GetInt("Aspect4Time"));

            return new ActionCardModel(
                data.GetString("Id"),
                data.GetString("Group"),
                data.GetString("Asset"),
                data.GetString("Name"),
                data.GetString("Description"),
                aspects,
                antiAspects,
                data.GetInt("Uniqueness"),
                0
            );
        }

        private IEntityCardModel GenerateEntityCardContentByCSV(Dictionary<string, object> data)
        {
            var aspects = new Dictionary<string, int>();

            if (!data.GetString("Aspect1").IsEmpty()) 
                aspects.Add(data.GetString("Aspect1"), data.GetInt("Aspect1Time"));
            if (!data.GetString("Aspect2").IsEmpty()) 
                aspects.Add(data.GetString("Aspect2"), data.GetInt("Aspect2Time"));
            if (!data.GetString("Aspect3").IsEmpty()) 
                aspects.Add(data.GetString("Aspect3"), data.GetInt("Aspect3Time"));
            if (!data.GetString("Aspect4").IsEmpty()) 
                aspects.Add(data.GetString("Aspect4"), data.GetInt("Aspect4Time"));   
            if (!data.GetString("Aspect5").IsEmpty()) 
                aspects.Add(data.GetString("Aspect5"), data.GetInt("Aspect5Time"));   
            if (!data.GetString("Aspect6").IsEmpty()) 
                aspects.Add(data.GetString("Aspect6"), data.GetInt("Aspect6Time"));

            var antiAspects = new Dictionary<string, int>();

            if (!data.GetString("AntiAspect1").IsEmpty()) 
                antiAspects.Add(data.GetString("AntiAspect1"), data.GetInt("AntiAspect1Time"));
            if (!data.GetString("AntiAspect2").IsEmpty()) 
                antiAspects.Add(data.GetString("AntiAspect2"), data.GetInt("AntiAspect2Time"));
            if (!data.GetString("AntiAspect3").IsEmpty()) 
                antiAspects.Add(data.GetString("AntiAspect3"), data.GetInt("AntiAspect3Time"));
            if (!data.GetString("AntiAspect4").IsEmpty()) 
                antiAspects.Add(data.GetString("AntiAspect4"), data.GetInt("AntiAspect4Time"));

            return new EntityCardModel(
                data.GetString("Id"),
                data.GetString("Asset"),
                data.GetString("Group"),
                data.GetString("Name"),
                data.GetString("Description"),
                aspects,
                antiAspects,
                data.GetInt("Move"),
                data.GetInt("Uniqueness")
            );
        }

        private IMergeResult GenerateMergeResultByCSV(Dictionary<string, object> data)
        {
            var aspectsToAdd = new Dictionary<string, int>();
            var antiAspectsToAdd = new Dictionary<string, int>();
            var aspectsToDelete = new List<string>();
            var antiAspectsToDelete = new List<string>();

            if (!data.GetString("AddAspect1").IsEmpty())
                aspectsToAdd.Add(data.GetString("AddAspect1"),data.GetInt("Aspect1Time"));
            if (!data.GetString("AddAspect2").IsEmpty())
                aspectsToAdd.Add(data.GetString("AddAspect2"),data.GetInt("Aspect2Time"));

            if (!data.GetString("AddAntiAspect1").IsEmpty())
                antiAspectsToAdd.Add(data.GetString("AddAntiAspect1Time"),data.GetInt("Aspect1Time"));
            if (!data.GetString("AddAntiAspect2").IsEmpty())
                antiAspectsToAdd.Add(data.GetString("AddAntiAspect2Time"),data.GetInt("Aspect2Time"));

            if (!data.GetString("DeleteAspect1").IsEmpty())
                aspectsToDelete.Add(data.GetString("DeleteAspect1"));
            if (!data.GetString("DeleteAspect2").IsEmpty())
                aspectsToDelete.Add(data.GetString("DeleteAspect2"));

            if (!data.GetString("DeleteAntiAspect1").IsEmpty())
                antiAspectsToDelete.Add(data.GetString("DeleteAntiAspect1"));
            if (!data.GetString("DeleteAntiAspect2").IsEmpty())
                antiAspectsToDelete.Add(data.GetString("DeleteAntiAspect2"));
            
            var IsEntityCardDestroyed = 
                data.GetString("DeleteEntity") != "" && data.GetString("DeleteEntity") != "0";

            return new MergeResult(
                data.GetString("AddEntity"),
                data.GetString("AddAction"),
                new KeyValuePair<string, int>(
                    data.GetString("AddEntitiesFromGroup"),
                    data.GetInt("CountNewActions")),
                new KeyValuePair<string, int>(
                    data.GetString("AddEntitiesFromGroup"),
                    data.GetInt("CountNewEntities")),
                aspectsToAdd,
                antiAspectsToAdd,
                aspectsToDelete,
                antiAspectsToDelete,
                IsEntityCardDestroyed,
                data.GetString("Log")
                );
        }
    }
}