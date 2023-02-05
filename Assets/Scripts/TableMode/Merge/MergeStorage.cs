namespace TableMode
{
    public class MergeStorage : IMergeStorage
    {
        private IActionCardView _actionCardView;
        private IEntityCardView _entityCardView;
        
        public void SetActionCardView(IActionCardView actionCardView)
        {
            _actionCardView = actionCardView;
        }

        public void SetEntityCardView(IEntityCardView entityCardView)
        {
            _entityCardView = entityCardView;
        }

        public void ClearActionCardView()
        {
            _actionCardView = null;
        }

        public void ClearEntityCardView()
        {
            _entityCardView = null;
        }

        (IActionCardView actionCardView, IEntityCardView entityCardView) IMergeStorage.GetMergePair()
        {
            return (_actionCardView, _entityCardView);
        }

        private void Clear()
        {
            _actionCardView = null;
            _entityCardView = null;
        }
    }
}