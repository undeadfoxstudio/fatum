using System.Collections.Generic;
using System.Linq;

namespace TableMode
{
    public class ActionCard : IActionCard
    {
        public string Id { get; }
        public string Name { get; }
        public IList<IAspect> Aspects { get; set; }
        public IList<IAspect> AntiAspects { get; set; }

        public ActionCard(
            string id, 
            IList<IAspect> aspects,
            IList<IAspect> antiAspects,
            string name)
        {
            Id = id;
            Aspects = aspects;
            AntiAspects = antiAspects;
            Name = name;
        }
        
        public IList<string> UpdateAspects()
        {
            var aspectsToDelete = Aspects
                .Where(a => a.Count == 1)
                .Select(a => a.Id);
            
            Aspects
                .Where(a => a.Count == 1)
                .ToList()
                .ForEach(a=>
                    AntiAspects.Remove(a));

            AntiAspects
                .Where(a => a.Count == 1)
                .ToList()
                .ForEach(a=>
                    AntiAspects.Remove(a));

            foreach (var aspect in Aspects)
                aspect.Update();

            foreach (var aspect in AntiAspects)
                aspect.Update();

            return aspectsToDelete.ToList();
        }
    }
}