using System.Collections.Generic;

namespace TableMode
{
    public class MergeResult : IMergeResult
    {
        public string EntityCardIdToAdd { get; }
        public string ActionCardIdToAdd { get; }
        public KeyValuePair<string, int> ActionsFromGroupToAdd { get; }
        public KeyValuePair<string, int> EntitiesFromGroupToAdd { get; }
        public IDictionary<string, int> AspectsToAdd { get; }
        public IDictionary<string, int> AntiAspectsToAdd { get; }
        public IEnumerable<string> AspectsToDelete { get; }
        public IEnumerable<string> AntiAspectsToDelete { get; }
        public string Log { get; }
        public bool IsEntityCardDestroyed { get; }

        public MergeResult(
            string entityCardIdToAdd, 
            string actionCardIdToAdd, 
            KeyValuePair<string, int> actionsFromGroupToAdd, 
            KeyValuePair<string, int> entitiesFromGroupToAdd, 
            IDictionary<string, int> aspectsToAdd, 
            IDictionary<string, int> antiAspectsToAdd, 
            IEnumerable<string> aspectsToDelete, 
            IEnumerable<string> antiAspectsToDelete, 
            bool isEntityCardDestroyed, 
            string log)
        {
            EntityCardIdToAdd = entityCardIdToAdd;
            ActionCardIdToAdd = actionCardIdToAdd;
            ActionsFromGroupToAdd = actionsFromGroupToAdd;
            EntitiesFromGroupToAdd = entitiesFromGroupToAdd;
            AspectsToAdd = aspectsToAdd;
            AntiAspectsToAdd = antiAspectsToAdd;
            AspectsToDelete = aspectsToDelete;
            AntiAspectsToDelete = antiAspectsToDelete;
            IsEntityCardDestroyed = isEntityCardDestroyed;
            Log = log;
        }
    }
}