using System.Collections.Generic;

namespace TableMode
{
    public interface IResult
    {
        string EntityCardIdToAdd { get; }
        string ActionCardIdToAdd { get; }
        KeyValuePair<string, int> ActionsFromGroupToAdd { get; }
        KeyValuePair<string, int> EntitiesFromGroupToAdd { get; }
        IDictionary<string, int> AspectsToAdd { get; }
        IDictionary<string, int> AntiAspectsToAdd { get; }
        IEnumerable<string> AspectsToDelete { get; }
        IEnumerable<string> AntiAspectsToDelete { get; }
        string Log { get; }
    }
}