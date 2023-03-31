using System.Collections.Generic;

namespace TableMode
{
    public struct AspectResult : IAspectResult
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
        public bool IsActionCardDestroyed { get; }

        public AspectResult(
            string entityCardIdToAdd, 
            string actionCardIdToAdd, 
            KeyValuePair<string, int> entitiesFromGroupToAdd, 
            IDictionary<string, int> aspectsToAdd, 
            IEnumerable<string> aspectsToDelete, 
            bool isEntityCardDestroyed,
            KeyValuePair<string, int> actionsFromGroupToAdd, 
            IDictionary<string, int> antiAspectsToAdd, 
            IEnumerable<string> antiAspectsToDelete, 
            bool isActionCardDestroyed, string log)
        {
            EntityCardIdToAdd = entityCardIdToAdd;
            ActionCardIdToAdd = actionCardIdToAdd;
            EntitiesFromGroupToAdd = entitiesFromGroupToAdd;
            AspectsToAdd = aspectsToAdd;
            AspectsToDelete = aspectsToDelete;
            IsEntityCardDestroyed = isEntityCardDestroyed;
            ActionsFromGroupToAdd = actionsFromGroupToAdd;
            AntiAspectsToAdd = antiAspectsToAdd;
            AntiAspectsToDelete = antiAspectsToDelete;
            IsActionCardDestroyed = isActionCardDestroyed;
            Log = log;
        }
    }
}