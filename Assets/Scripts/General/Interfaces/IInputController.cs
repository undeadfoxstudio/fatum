using System;
using UnityEngine;

namespace TableMode
{
    public interface IInputController
    {
        public event Action<RaycastHit> OnRaycast;
        public event Action<Vector3> OnClickStart;
        public event Action<Vector3> OnClickEnd;
        public event Action<float> OnScroll;
    }
}