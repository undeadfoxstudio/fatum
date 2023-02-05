using UnityEngine;

namespace TableMode
{
    public interface IView
    {
        void Hover(Vector3 point);
        void UnHover();
        void Move(Vector3 point, bool local = false);
        void MoveImmediately(Vector3 point, bool local = false);
    }
}