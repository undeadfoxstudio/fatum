namespace TableMode
{
    public class AspectViewFactory : IAspectViewFactory
    {
        private readonly AspectBehavior _aspectBehaviorPrefab;
        private readonly IAspectFactory _aspectFactory;
        private readonly IAssetsProvider _assetsProvider;

        public AspectViewFactory(
            AspectBehavior aspectBehaviorPrefab,
            IAspectFactory aspectFactory,
            IAssetsProvider assetsProvider)
        {
            _aspectBehaviorPrefab = aspectBehaviorPrefab;
            _aspectFactory = aspectFactory;
            _assetsProvider = assetsProvider;
        }

        public IAspectView CreateAspect(string id, int count = 0)
        {
            var behavior = UnityEngine.Object.Instantiate(_aspectBehaviorPrefab);
            var aspect = _aspectFactory.Create(id, count);
            var activeAspectSprite = _assetsProvider.GetActiveAspectSprite(aspect.Asset);
            var inactiveAspectSprite = _assetsProvider.GetInactiveAspectSprite(aspect.Asset);

            return new AspectView(
                aspect, 
                behavior,
                activeAspectSprite,
                inactiveAspectSprite);
        }

        public IAspectView CreateAntiAspect(string id, int count = 0)
        {
            var behavior = UnityEngine.Object.Instantiate(_aspectBehaviorPrefab);
            var aspect = _aspectFactory.Create(id, count);
            var activeAspectSprite = _assetsProvider.GetAntiAspectSprite(aspect.Asset);
            var inactiveAspectSprite = _assetsProvider.GetAntiAspectSprite(aspect.Asset);
 
            return new AspectView(
                aspect, 
                behavior,
                activeAspectSprite,
                inactiveAspectSprite);
        }
    }
}