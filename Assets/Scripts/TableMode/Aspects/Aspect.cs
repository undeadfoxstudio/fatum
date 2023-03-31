using UnityEngine;

namespace TableMode
{
    public class Aspect : IAspect
    {
        public bool IsActive { get; set; }
        public string Asset { get; }
        public string Id { get; }
        public string Name { get; }
        public string Description { get; }
        public Color GradientColor { get; }
        public int Count { get; set; }

        public Aspect(
            string id,
            string name,
            int count, 
            string asset, 
            string description, 
            Color gradientColor)
        {
            Id = id;
            Name = name;
            Count = count;
            Asset = asset;
            Description = description;
            GradientColor = gradientColor;
            IsActive = true;
        }

        public void Update()
        {
            IsActive = true;
            if (Count > 1) Count--;
        }
    }
}