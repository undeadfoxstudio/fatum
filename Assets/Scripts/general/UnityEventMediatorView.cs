using System;
using UnityEngine;

namespace TableMode
{
    public class UnityEventMediatorView : MonoBehaviour
    {
        private Action<float> _update;

        public void Listen(Action<float> update)
        {
            _update = update;
        }

        private void Update()
        {
            _update?.Invoke(Time.deltaTime);
        }
    }
}