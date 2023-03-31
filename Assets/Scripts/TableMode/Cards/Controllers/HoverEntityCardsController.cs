using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TableMode
{
    public class HoverEntityCardsController : IHoverEntityCardsController
    {
        private readonly IInputController _inputController;
        private readonly IDictionary<Collider, IEntityCardView> _tableCards = new Dictionary<Collider, IEntityCardView>();

        private Collider _currentCardCollider;
        private bool IsEnabled = true;

        public event Action<IEntityCardView> OnActionCardViewHover;
        public event Action OnActionCardViewLeave;

        public HoverEntityCardsController(ICardViewFactory cardViewFactory,
            IInputController inputController)
        {
            _inputController = inputController;
            _inputController.OnRaycast += InputControllerOnRaycast;
            _inputController.OnClickStart += InputControllerOnClickStart;
            _inputController.OnClickEnd += InputControllerOnClickEnd;

            cardViewFactory.OnCreateEntityCard += OnAddCard;
        }

        private void InputControllerOnClickEnd(Vector3 obj)
        {
            IsEnabled = true;
        }

        private void InputControllerOnClickStart(Vector3 obj)
        {
            IsEnabled = false;
        }

        private void InputControllerOnRaycast(RaycastHit raycastHit)
        {
            if (!IsEnabled) return;

            var view =_tableCards
                .FirstOrDefault(c => c.Key == raycastHit.collider);

            if (view.Key == null)
            {
                if (_currentCardCollider != null)
                {
                    _tableCards[_currentCardCollider].UnHover();
                    OnActionCardViewLeave?.Invoke();
                    _currentCardCollider = null;
                }

                return;
            }

            OnActionCardViewHover?.Invoke(view.Value);

            view.Value.Hover(raycastHit.point);
  
            if (_currentCardCollider == null)
            {
                _currentCardCollider = view.Key;

                return;
            }

            if (_currentCardCollider != view.Key)
            {
                _tableCards[_currentCardCollider].UnHover();
                OnActionCardViewLeave?.Invoke();
                _currentCardCollider = view.Key;
            }
        }

        private void OnAddCard(IEntityCardView entityCard)
        {
            _tableCards.Add(entityCard.Collider, entityCard);
        }

        public IEnumerable<IEntityCardView> GetHoveredCard()
        {
            var hoveredCard = new List<IEntityCardView>();

            if (_currentCardCollider != null) 
                hoveredCard.Add(_tableCards[_currentCardCollider]);

            return hoveredCard;
        }
    }
}