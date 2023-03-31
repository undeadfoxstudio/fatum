using System.Linq;

namespace TableMode
{
    public class InspectorController : IInspectorController
    {
        private readonly IHoverActionCardsController _hoverActionCardsController;
        private readonly IHoverEntityCardsController _hoverEntityCardsController;
        private readonly IUIController _uiController;
        private ICardView _hoveredCard;

        public InspectorController(
            IHoverActionCardsController hoverActionCardsController,
            IHoverEntityCardsController hoverEntityCardsController,
            IUIController uiController)
        {
            _uiController = uiController;
            _hoverActionCardsController = hoverActionCardsController;
            _hoverEntityCardsController = hoverEntityCardsController;

            _hoverEntityCardsController.OnActionCardViewHover += HoverCardControllersOnCardViewHover;
            _hoverEntityCardsController.OnActionCardViewLeave += HoverCardsControllerOnActionCardViewLeave;
            _hoverActionCardsController.OnActionCardViewHover += HoverCardControllersOnCardViewHover;
            _hoverActionCardsController.OnActionCardViewLeave += HoverCardsControllerOnActionCardViewLeave;
        }

        private void HoverCardControllersOnCardViewHover(ICardView cardView)
        {
            if (_hoveredCard == cardView) return;

            _uiController.ShowInspector();
            _uiController.SetCurrentCardCaption(cardView.Name);
            _uiController.SetCurrentCardDescription(cardView.Description);
            _uiController.SetAspects(cardView.Aspects.ToList());
            _uiController.SetAspects(cardView.AntiAspects.ToList(), true);

            _hoveredCard = cardView;
        }
        private void HoverCardsControllerOnActionCardViewLeave()
        {
            _uiController.HideInspector();

            _hoveredCard = null;
        }
    }
}