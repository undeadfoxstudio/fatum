
using UnityEngine;

namespace TableMode
{
    public interface IAspect
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        Color GradientColor { get; }
        int Count { get; set; }
        bool IsActive { get; set; }
        string Asset { get; }
        void Update();
    }
}