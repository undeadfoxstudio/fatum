using System.Collections.Generic;

namespace TableMode
{
    public interface IContentProvider
    {
        IList<string> GetActionIdsByGroup(string groupId);
        IEntityCardModel GetEntityById(string id);
        IActionCardModel GetActionById(string id);
        IAspectModel GetAspectById(string id);
        IEnumerable<IMergeRuleModel> MergeRuleModels();
        IEnumerable<IAspectRuleModel> AspectRuleModels();
        IEnumerable<string> GetEntityIdsFromGroup(string groupId);
    }
}