using System.Collections.Generic;
using UnityEngine;

namespace TableMode
{
    public interface ICardView : IView
    {
        Collider Collider { get; }
        string Id { get; }
        string Name { get; }
        string Description { get; }
        Vector3 Position { get; }
        IEnumerable<IAspect> Aspects { get; }
        IEnumerable<IAspect> AntiAspects { get; }
        Vector3 OffsetMove { get; }

        void NextStep();
        void Destroy();
        void AddAspect(IAspectView aspect);
        void RemoveAspect(string aspectId);
        void RemoveAntiAspect(string aspectId);
    }
}