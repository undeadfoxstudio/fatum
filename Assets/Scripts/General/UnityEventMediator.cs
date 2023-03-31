using System.Collections.Generic;
using UnityEngine;

namespace TableMode
{
    public class UnityEventMediator 
    {
        private readonly List<IUpdatable> _updatables;
        private readonly UnityEventMediatorView _unityEventMediatorView;

        protected UnityEventMediator(List<IUpdatable> updatables)
        {
            _updatables = updatables;

            _unityEventMediatorView = new GameObject("UnityEventMediator").AddComponent<UnityEventMediatorView>();
            _unityEventMediatorView.Listen(Update);
        }

        private void Update(float deltaTime)
        {
            foreach (var item in _updatables)
                item.CustomUpdate(deltaTime);
        }
    }
}