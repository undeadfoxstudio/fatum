using UnityEngine;

namespace TableMode
{
    public class CameraController : MonoBehaviour, ICameraView
    {
        private Camera _mainCamera;

        private Vector3 _startCameraPosition;
        private Vector3 _currentCameraPosition;
        private Vector3 _newCameraPosition;
        private Vector3 _firstDragPosition;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _newCameraPosition = transform.position;
        }

        public void SetParent(Transform parent)
        {
            throw new System.NotImplementedException();
        }

        public void Hover(Vector3 point)
        {
            _startCameraPosition = point;
        }

        public void UnHover()
        {

        }

        public void Move(Vector3 point, bool local = false)
        {

        }

        public void MoveImmediately(Vector3 point, bool local = false)
        {
            _currentCameraPosition = point;

            if (_startCameraPosition != _currentCameraPosition)
            {
                _newCameraPosition = _startCameraPosition - _currentCameraPosition;
                _newCameraPosition.y = 0;
                transform.position += _newCameraPosition;
            }

            _currentCameraPosition = _startCameraPosition;
        }

        public float Distance
        {
            get => _mainCamera.transform.position.y;
            set
            {
                var cameraTransform = _mainCamera.transform;

                cameraTransform.position += cameraTransform.forward * value;
            }
        }
    }
}