using System;
using System.Collections.Generic;
using UnityEngine;

namespace TableMode
{
    public class MoveActionCardsController : IMoveActionCardsController, IUpdatable
    {
        private readonly IInputController _inputController;
        private readonly IHoverActionCardsController _hoverActionCardsController;
        private readonly IHandController _handController;
        private readonly ITableController _tableController;
        private readonly ITableProvider _tableProvider;

        private readonly BoxCollider _boxCollider;
        private IList<IActionCardView> _selectedCard = new List<IActionCardView>();
        private bool _isMoving;

        public event Action OnActionCardDrop;

        public MoveActionCardsController(
            BoxCollider boxCollider,
            IInputController inputController, 
            IHoverActionCardsController hoverActionCardsController,
            ITableProvider tableProvider,
            IHandController handController,
            ITableController tableController)
        {
            _boxCollider = boxCollider;
            _inputController = inputController;
            _hoverActionCardsController = hoverActionCardsController;
            _tableProvider = tableProvider;
            _handController = handController;
            _tableController = tableController;

            _inputController.OnClickStart += InputControllerOnClickStart;
            _inputController.OnClickEnd += InputControllerOnClickEnd;
        }
        
        private void InputControllerOnClickEnd(Vector3 obj)
        {
            _tableController.SwitchCardState(CardReady.ToDrag);
            _isMoving = false;

            if (_selectedCard.Count == 0) return;

            foreach (var actionCardView in _selectedCard)
                OnActionCardDrop?.Invoke();

            _selectedCard.Clear();
        }

        private void InputControllerOnClickStart(Vector3 position)
        {
            _tableController.SwitchCardState(CardReady.ToMerge);
            
            var hoveredCard = _hoverActionCardsController.GetHoveredCard();

            foreach (var cardView in hoveredCard)
            {
                _selectedCard.Add(cardView);
                _handController.CatchCard(cardView);
                _handController.ArrangeCard();
            }

            _isMoving = true;
        }

        public void CustomUpdate(float deltaTime)
        {
            if (!_isMoving || _selectedCard.Count == 0) return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (_boxCollider.Raycast(ray, out var hit, 100.0f))
            {
                foreach (var cardView in _selectedCard)
                    cardView.MoveImmediately(RestrictNewCardPosition(hit.point), true);
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
}