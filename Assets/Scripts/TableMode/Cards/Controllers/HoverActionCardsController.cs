using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TableMode
{
    public class HoverActionCardsController : IHoverActionCardsController
    {
        private readonly IInputController _inputController;
        private readonly IDictionary<Collider, IActionCardView> _handCards = new Dictionary<Collider, IActionCardView>();

        private Collider _currentCardCollider;
        private bool IsEnabled = true;
        
        public event Action<IActionCardView> OnActionCardViewHover;
        public event Action OnActionCardViewLeave;

        public HoverActionCardsController(ICardViewFactory cardViewFactory,
            IInputController inputController)
        {
            _inputController = inputController;
            _inputController.OnRaycast += InputControllerOnRaycast;
            _inputController.OnClickStart += InputControllerOnClickStart;
            _inputController.OnClickEnd += InputControllerOnClickEnd;

            cardViewFactory.OnCreateActionCard += OnAddCard;
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

            var view =_handCards
                .FirstOrDefault(c => c.Key == raycastHit.collider);

            if (view.Key == null)
            {
                if (_currentCardCollider != null)
                {
                    _handCards[_currentCardCollider].UnHover();
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
                _handCards[_currentCardCollider].UnHover();
                OnActionCardViewLeave?.Invoke();
                _currentCardCollider = view.Key;
            }
        }

        private void OnAddCard(IActionCardView entityCard)
        {
            _handCards.Add(entityCard.Collider, entityCard);
            entityCard.OnDestroy += EntityCardOnDestroy;
        }

        private void EntityCardOnDestroy(IActionCardView cardView)
        {
            _currentCardCollider = null;
            
            _handCards.Remove(_handCards.FirstOrDefault(v => v.Value == cardView));
        }

        public IEnumerable<IActionCardView> GetHoveredCard()
        {
            var hoveredCard = new List<IActionCardView>();

            if (_currentCardCollider != null) 
                hoveredCard.Add(_handCards[_currentCardCollider]);

            return hoveredCard;
        }
    }
}