using System.Collections.Generic;
using UnityEngine;

namespace TableMode
{
    public interface ICardBehavior
    {
        public List<GameObject> slots { get; }
        public Transform BehaviorTransform { get; }
        public Collider HoverCollider { get; }
        public void Hover();
        public void UnHover();
        public void Destroy();
        public void SetGradient(Texture2D texture2D);
    }
}