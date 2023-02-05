using System.Collections.Generic;

namespace TableMode
{
    public interface IActionCard
    {
        string Id { get; }
        string Name { get; }
        IList<IAspect> Aspects { set; get; }
        IList<IAspect> AntiAspects { get; set; }
        IList<string> UpdateAspects();
    }
}