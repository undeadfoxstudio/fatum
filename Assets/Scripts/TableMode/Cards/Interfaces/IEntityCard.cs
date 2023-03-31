using System.Collections.Generic;

namespace TableMode
{
    public interface IEntityCard
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        IList<IAspect> Aspects { set; get; }
        IList<IAspect> AntiAspects { get; set; }
        void NextStep();
        void AddAspect(IAspect aspect);
    }
}