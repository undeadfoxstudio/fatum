using System.Collections.Generic;
using UnityEngine;

namespace TableMode
{
    public class MoveEntityCardsController : IMoveEntityCardsController, IUpdatable
    {
        private readonly BoxCollider _boxCollider;
        private readonly IInputController _inputController;
        private readonly IHoverEntityCardsController _hoverEntityCardsController;
        private readonly ITableController _tableController;
        private readonly ITableProvider _tableProvider;

        private IList<IEntityCardView> _selectedCard = new List<IEntityCardView>();
        private bool _isMoving;

        public MoveEntityCardsController(
            BoxCollider boxCollider,
            IInputController inputController, 
            IHoverEntityCardsController hoverEntityCardsController,
            ITableController tableController,
            ITableProvider tableProvider)
        {
            _boxCollider = boxCollider;
            _inputController = inputController;
            _hoverEntityCardsController = hoverEntityCardsController;
            _tableController = tableController;
            _tableProvider = tableProvider;

            _inputController.OnClickStart += InputControllerOnClickStart;
            _inputController.OnClickEnd += InputControllerOnClickEnd;
        }
        
        private void InputControllerOnClickStart(Vector3 position)
        {
            var hoveredCard = _hoverEntityCardsController.GetHoveredCard();

            foreach (var cardView in hoveredCard)
            {
                _tableController.CatchCard(cardView);
                _selectedCard.Add(cardView);
            }

            _isMoving = true;
        }
        
        private void InputControllerOnClickEnd(Vector3 obj)
        {
            _isMoving = false;

            if (_selectedCard.Count == 0) return;

            foreach (var cardView in _selectedCard)
                cardView.Move(_tableController.TakeCard(cardView));

            _selectedCard.Clear();
        }

        public void CustomUpdate(float deltaTime)
        {
            if (!_isMoving) return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (_boxCollider.Raycast(ray, out var hit, 100.0f))
            {
                foreach (var cardView in _selectedCard)
                    cardView.MoveImmediately(RestrictNewCardPosition(hit.point));
            }
        }

        private Vector3 RestrictNewCardPosition(Vector3 newPosition)
        {
            var maxY = _tableProvider.Collider.center.z + _tableProvider.Collider.size.z / 2;
            var minY = _tableProvider.Collider.center.z - _tableProvider.Collider.size.z /2;
            var maxX = _tableProvider.Collider.center.x + _tableProvider.Collider.size.x / 2;
            var minX = _tableProvider.Collider.center.x - _tableProvider.Collider.size.x / 2;

            if (newPosition.x > maxY) newPosition.x = _tableProvider.Collider.center.z + _tableProvider.Collider.size.z / 2;
            if (newPosition.x < minY) newPosition.x = _tableProvider.Collider.center.z - _tableProvider.Collider.size.z / 2;
            if (newPosition.z > maxX) newPosition.z = _tableProvider.Collider.center.x + _tableProvider.Collider.size.x / 2;
            if (newPosition.z < minX) newPosition.z = _tableProvider.Collider.center.x - _tableProvider.Collider.size.x / 2;

            newPosition += new Vector3(0, 0.1f, 0);

            return newPosition;
        }
    }

    public interface IMoveEntityCardsController {}
}