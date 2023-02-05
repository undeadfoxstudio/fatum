using UnityEngine;

namespace TableMode
{
    public interface ICardBehavior
    {
        public Transform BehaviorTransform { get; }
        public Collider HoverCollider { get; }
        public void Hover();
        public void UnHover();
        public void Destroy();
    }
}