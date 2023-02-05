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

        private List<IMergeRuleModel> _mergeRules = new List<IMergeRuleModel>();
        private List<IAspectModel> _aspects = new List<IAspectModel>();
        private List<IActionCardModel> _actionCards = new List<IActionCardModel>();
        private List<IEntityCardModel> _entityCards = new List<IEntityCardModel>();
        private List<IAspectRuleModel> _aspectRules = new List<IAspectRuleModel>();

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
                {
                    _aspectRules = CSVReader.Read(_contentConfig.AspectRules.text)
                        .Select(GenerateAspectRuleContentByCSV)
                        .ToList();

                    Debug.Log(JsonConvert.SerializeObject(_aspectRules));
                    Debug.Log(_aspectRules.Count);
                }

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
                data.GetString("aspect1"),
                data.GetString("aspect2"),
                data.GetString("aspect3"),
                data.GetString("aspect4"),
                data.GetString("aspect5")
            }.Where(a => a != "");

            return new AspectTrigger(
                data.GetString("entity"),
                data.GetString("action"),
                data.GetString("expiringAspect"),
                aspects,
                data.GetInt("weight")
            );
        }

        private IAspectResult GenerateAspectResultByCSV(Dictionary<string, object> data)
        {
            var aspectsToAdd = new Dictionary<string, int>();
            var antiAspectsToAdd = new Dictionary<string, int>();
            var aspectsToDelete = new List<string>();
            var antiAspectsToDelete = new List<string>();

            if (!data.GetString("addAspect1").IsEmpty())
                aspectsToAdd.Add(data.GetString("addAspect1"),data.GetInt("aspect1Time"));
            if (!data.GetString("addAspect2").IsEmpty())
                aspectsToAdd.Add(data.GetString("addAspect2"),data.GetInt("aspect2Time"));

            if (!data.GetString("addAntiAspect1").IsEmpty())
                antiAspectsToAdd.Add(data.GetString("addAntiAspect1Time"),data.GetInt("aspect1Time"));
            if (!data.GetString("addAntiAspect2").IsEmpty())
                antiAspectsToAdd.Add(data.GetString("addAntiAspect2Time"),data.GetInt("aspect2Time"));

            if (!data.GetString("deleteAspect1").IsEmpty())
                aspectsToDelete.Add(data.GetString("deleteAspect1"));
            if (!data.GetString("deleteAspect2").IsEmpty())
                aspectsToDelete.Add(data.GetString("deleteAspect2"));

            if (!data.GetString("deleteAntiAspect1").IsEmpty())
                antiAspectsToDelete.Add(data.GetString("deleteAntiAspect1"));
            if (!data.GetString("deleteAntiAspect2").IsEmpty())
                antiAspectsToDelete.Add(data.GetString("deleteAntiAspect2"));
            
            var IsEntityCardDestroyed = 
                data.GetString("deleteEntity") != "" && data.GetString("deleteEntity") != "0";

            var IsActionCardDestroyed =
                data.GetString("deleteAction") != "" && data.GetString("deleteAction") != "0";
            
            return new AspectResult(
                data.GetString("addEntity"),
                data.GetString("addAction"),
                new KeyValuePair<string, int>(
                    data.GetString("addEntitiesFromGroup"),
                    data.GetInt("countNewEntities")),
                aspectsToAdd,
                aspectsToDelete,
                IsEntityCardDestroyed,
                new KeyValuePair<string, int>(
                    data.GetString("addActionsFromGroup"),
                    data.GetInt("countNewActions")),
                antiAspectsToAdd,
                antiAspectsToDelete,
                IsActionCardDestroyed
                );
        }

        private IMergeTrigger GenerateMergeTriggerByCSV(Dictionary<string, object> data)
        {
            var aspects = new List<string>
            {
                data.GetString("aspect1"),
                data.GetString("aspect2"),
                data.GetString("aspect3"),
                data.GetString("aspect4"),
                data.GetString("aspect5"),
                data.GetString("aspect6")
            }.Where(a => a != "");

            return new MergeTrigger(
                data.GetString("entity"),
                data.GetString("action"),
                aspects,
                data.GetInt("weight"));
        }

        private IAspectModel GenerateAspectContentByCSV(Dictionary<string, object> data)
        {
            return new AspectModel
            (
                data.GetString("id"),
                data.GetInt("order"),
                data.GetString("group"),
                data.GetString("name"),
                data.GetString("description"),
                GenerateAspectResultByCSV(data)
            );
        }

        private IActionCardModel GenerateActionCardContentByCSV(Dictionary<string, object> data)
        {
            var aspects = new Dictionary<string, int>();

            if (!data.GetString("aspect1").IsEmpty()) 
                aspects.Add(data.GetString("aspect1"), data.GetInt("aspect1Time"));
            if (!data.GetString("aspect2").IsEmpty()) 
                aspects.Add(data.GetString("aspect2"), data.GetInt("aspect2Time"));
            if (!data.GetString("aspect3").IsEmpty()) 
                aspects.Add(data.GetString("aspect3"), data.GetInt("aspect3Time"));
            if (!data.GetString("aspect4").IsEmpty()) 
                aspects.Add(data.GetString("aspect4"), data.GetInt("aspect4Time"));
            if (!data.GetString("aspect5").IsEmpty()) 
                aspects.Add(data.GetString("aspect5"), data.GetInt("aspect5Time")); 
            if (!data.GetString("aspect6").IsEmpty()) 
                aspects.Add(data.GetString("aspect6"), data.GetInt("aspect6Time"));

            var antiAspects = new Dictionary<string, int>();

            if (!data.GetString("antiAspect1").IsEmpty()) 
                antiAspects.Add(data.GetString("antiAspect1"), data.GetInt("aspect1Time"));
            if (!data.GetString("antiAspect2").IsEmpty()) 
                antiAspects.Add(data.GetString("antiAspect2"), data.GetInt("aspect2Time"));
            if (!data.GetString("antiAspect3").IsEmpty()) 
                antiAspects.Add(data.GetString("antiAspect3"), data.GetInt("aspect3Time"));
            if (!data.GetString("antiAspect4").IsEmpty()) 
                antiAspects.Add(data.GetString("antiAspect4"), data.GetInt("aspect4Time"));

            return new ActionCardModel(
                data.GetString("id"),
                data.GetString("group"),
                data.GetString("name"),
                data.GetString("description"),
                aspects,
                antiAspects,
                data.GetInt("uniqueness"),
                0
            );
        }

        private IEntityCardModel GenerateEntityCardContentByCSV(Dictionary<string, object> data)
        {
            var aspects = new Dictionary<string, int>();

            if (!data.GetString("aspect1").IsEmpty()) 
                aspects.Add(data.GetString("aspect1"), data.GetInt("aspect1Time"));
            if (!data.GetString("aspect2").IsEmpty()) 
                aspects.Add(data.GetString("aspect2"), data.GetInt("aspect2Time"));
            if (!data.GetString("aspect3").IsEmpty()) 
                aspects.Add(data.GetString("aspect3"), data.GetInt("aspect3Time"));
            if (!data.GetString("aspect4").IsEmpty()) 
                aspects.Add(data.GetString("aspect4"), data.GetInt("aspect4Time"));   
            if (!data.GetString("aspect5").IsEmpty()) 
                aspects.Add(data.GetString("aspect5"), data.GetInt("aspect5Time"));   
            if (!data.GetString("aspect6").IsEmpty()) 
                aspects.Add(data.GetString("aspect6"), data.GetInt("aspect6Time"));

            var antiAspects = new Dictionary<string, int>();

            if (!data.GetString("antiAspect1").IsEmpty()) 
                antiAspects.Add(data.GetString("antiAspect1"), data.GetInt("antiAspect1Time"));
            if (!data.GetString("antiAspect2").IsEmpty()) 
                antiAspects.Add(data.GetString("antiAspect2"), data.GetInt("antiAspect2Time"));
            if (!data.GetString("antiAspect3").IsEmpty()) 
                antiAspects.Add(data.GetString("antiAspect3"), data.GetInt("antiAspect3Time"));
            if (!data.GetString("antiAspect4").IsEmpty()) 
                antiAspects.Add(data.GetString("antiAspect4"), data.GetInt("antiAspect4Time"));

            return new EntityCardModel(
                data.GetString("id"),
                data.GetString("group"),
                data.GetString("name"),
                data.GetString("description"),
                aspects,
                antiAspects,
                data.GetInt("move"),
                data.GetInt("uniqueness")
            );
        }

        private IMergeResult GenerateMergeResultByCSV(Dictionary<string, object> data)
        {
            var aspectsToAdd = new Dictionary<string, int>();
            var antiAspectsToAdd = new Dictionary<string, int>();
            var aspectsToDelete = new List<string>();
            var antiAspectsToDelete = new List<string>();

            if (!data.GetString("addAspect1").IsEmpty())
                aspectsToAdd.Add(data.GetString("addAspect1"),data.GetInt("aspect1Time"));
            if (!data.GetString("addAspect2").IsEmpty())
                aspectsToAdd.Add(data.GetString("addAspect2"),data.GetInt("aspect2Time"));

            if (!data.GetString("addAntiAspect1").IsEmpty())
                antiAspectsToAdd.Add(data.GetString("addAntiAspect1Time"),data.GetInt("aspect1Time"));
            if (!data.GetString("addAntiAspect2").IsEmpty())
                antiAspectsToAdd.Add(data.GetString("addAntiAspect2Time"),data.GetInt("aspect2Time"));

            if (!data.GetString("deleteAspect1").IsEmpty())
                aspectsToDelete.Add(data.GetString("deleteAspect1"));
            if (!data.GetString("deleteAspect2").IsEmpty())
                aspectsToDelete.Add(data.GetString("deleteAspect2"));

            if (!data.GetString("deleteAntiAspect1").IsEmpty())
                antiAspectsToDelete.Add(data.GetString("deleteAntiAspect1"));
            if (!data.GetString("deleteAntiAspect2").IsEmpty())
                antiAspectsToDelete.Add(data.GetString("deleteAntiAspect2"));
            
            var IsEntityCardDestroyed = 
                data.GetString("deleteEntity") != "" && data.GetString("deleteEntity") != "0";

            return new MergeResult(
                data.GetString("addEntity"),
                data.GetString("addAction"),
                new KeyValuePair<string, int>(
                    data.GetString("addEntitiesFromGroup"),
                    data.GetInt("countNewEntities")),
                new KeyValuePair<string, int>(
                    data.GetString("addActionsFromGroup"),
                    data.GetInt("countNewActions")),
                aspectsToAdd,
                antiAspectsToAdd,
                aspectsToDelete,
                antiAspectsToDelete,
                IsEntityCardDestroyed
                );
        }
    }
}