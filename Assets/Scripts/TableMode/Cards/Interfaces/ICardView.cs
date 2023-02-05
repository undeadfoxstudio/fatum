using System.Collections.Generic;
using UnityEngine;

namespace TableMode
{
    public interface ICardView : IView
    {
        Collider Collider { get; }
        string Id { get; }
        Vector3 Position { get; }
        IEnumerable<IAspect> Aspects { get; }
        IEnumerable<IAspect> AntiAspects { get; }

        void NextStep();
        void Destroy();
        void AddAspect(IAspect aspect);
        void RemoveAspect(string aspectId);
        void Rotate(float angle);
    }
}