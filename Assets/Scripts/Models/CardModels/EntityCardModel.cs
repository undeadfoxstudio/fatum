﻿using System.Collections.Generic;

namespace TableMode
{
    public struct EntityCardModel : IEntityCardModel
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

        public EntityCardModel
            (string id,
                string asset,
                string group,
                string name,
                string description,
                IDictionary<string, int> aspects,
                IDictionary<string, int> antiAspects,
                int uniqueness, 
                int chance)
        {
            Id = id;
            Group = group;
            Name = name;
            Description = description;
            Aspects = aspects;
            AntiAspects = antiAspects;
            Uniqueness = uniqueness;
            Chance = chance;
            Asset = asset;
        }
    }
}