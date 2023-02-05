using System.Collections.Generic;

namespace TableMode
{
    public interface IActionCardModel
    {
        public string Id { get; }
        public string Group { get; }
        public int Chance { get; }
        public string Name { get; }
        public string Description { get; }
        public IDictionary<string, int> Aspects { get; }
        public IDictionary<string, int> AntiAspects { get; }
        public int Uniqueness { get; }
    }
}