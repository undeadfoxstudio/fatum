using System.Collections.Generic;
using System.Linq;

namespace TableMode
{
    public class EntityCard : IEntityCard
    {
        public string Id { get; }
        public string Name { get; }
        public string Description { get; }
        public IList<IAspect> Aspects { get; set; }
        public IList<IAspect> AntiAspects { get; set; }

        public EntityCard(
            string id, 
            string name, 
            IList<IAspect> aspects,
            IList<IAspect> antiAspects,
            string description)
        {
            Id = id;
            Name = name;
            Aspects = aspects;
            AntiAspects = antiAspects;
            Description = description;
        }

        public void NextStep()
        {
            Aspects
                .Where(a => a.Count == 1)
                .ToList()
                .ForEach(a=>
                    Aspects.Remove(a));

            AntiAspects
                .Where(a => a.Count == 1)
                .ToList()
                .ForEach(a=>
                    AntiAspects.Remove(a));

            foreach (var aspect in Aspects)
                aspect.Update();

            foreach (var aspect in AntiAspects)
                aspect.Update();
        }

        public void AddAspect(IAspect aspect)
        {
            if (Aspects.All(a => a.Id != aspect.Id))
                Aspects.Add(aspect);
            else
                Aspects.First(a => a.Id == aspect.Id).Count += aspect.Count;
        }
    }
}