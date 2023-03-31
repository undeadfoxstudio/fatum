using System.Collections.Generic;

namespace TableMode
{
    public interface IEntityCardModel
    {
        public string Id { get; }
        public string Asset { get; }
        public string Group { get; }
        public int Chance { get; }
        public string Name { get; }
        public string Description { get; }
        public IDictionary<string, int> Aspects { get; }
        public IDictionary<string, int> AntiAspects { get; }
        public int Uniqueness { get; }
    }
}