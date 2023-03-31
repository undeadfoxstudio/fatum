using UnityEngine;

namespace TableMode
{
    public class MoveCameraController : IMoveCameraController, IUpdatable
    {
        private readonly IInputController _inputController;
        private readonly BoxCollider _collider;
        private readonly ICameraView _cameraView;
        private readonly CameraConfig _cameraConfig;
        private bool _isSelecting;
        private bool _isMoving;

        public MoveCameraController(
            IInputController inputController, 
            BoxCollider collider,
            ICameraView cameraView,
            CameraConfig cameraConfig)
        {
            _inputController = inputController;
            _collider = collider;
            _cameraView = cameraView;
            _cameraConfig = cameraConfig;

            _inputController.OnRaycast += InputControllerOnRaycast;
            _inputController.OnClickStart += InputControllerOnClickStart;
            _inputController.OnClickEnd += InputControllerOnClickEnd;
            _inputController.OnScroll += InputControllerOnScroll;
        }

        private void InputControllerOnScroll(float deltaY)
        {
            if (deltaY > 0 && _cameraView.Distance > _cameraConfig.MinScrollDistance)
                _cameraView.Distance = deltaY * _cameraConfig.ScrollSpeed;
            
            if (deltaY < 0 && _cameraView.Distance < _cameraConfig.MaxScrollDistance)
                _cameraView.Distance = deltaY * _cameraConfig.ScrollSpeed;
        }

        private void InputControllerOnClickStart(Vector3 position)
        {
            _isMoving = true;
        }

        private void InputControllerOnClickEnd(Vector3 position)
        {
            _isMoving = false;
        }

        private void InputControllerOnRaycast(RaycastHit raycastHit)
        {
            if (_isMoving) return;

            if (_collider == raycastHit.collider)
            {
                _isSelecting = true;
                _cameraView.Hover(raycastHit.point);
            }
            else
            {
                _isSelecting = false;
            }
        }

        public void CustomUpdate(float deltaTime)
        {
            if (!_isMoving || !_isSelecting) return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (_collider.Raycast(ray, out var hit, 100.0f))
            {
                _cameraView.MoveImmediately(hit.point);
            }
        }
    }
}