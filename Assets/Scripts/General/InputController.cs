using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TableMode
{
    public class InputController : IUpdatable, IInputController
    {
        public event Action<RaycastHit> OnRaycast;
        public event Action<Vector3> OnClickStart;
        public event Action<Vector3> OnClickEnd;
        public event Action<float> OnScroll;

        public void CustomUpdate(float deltaTime)
        {

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            var eventData = new PointerEventData(EventSystem.current) {
                position = Input.mousePosition
            };
            var raycastResults = new List<RaycastResult>();

            EventSystem.current.RaycastAll(eventData, raycastResults );
            
            foreach (var currentRaycastResult in raycastResults)
            {
                if (currentRaycastResult.gameObject.layer == LayerMask.NameToLayer("MAINUI"))
                    return;
            }

            if (Input.GetMouseButtonDown(0)) OnClickStart?.Invoke(Input.mousePosition);
            if (Input.GetMouseButtonUp(0)) OnClickEnd?.Invoke(Input.mousePosition);
            if (Input.mouseScrollDelta.y != 0) OnScroll?.Invoke(Input.mouseScrollDelta.y);

            if (Physics.Raycast(ray, out var entry, 10000))
                OnRaycast?.Invoke(entry);
        }
    }
}