using UnityEngine;

namespace TableMode
{
    public class AspectView : IAspectView
    {
        public IAspect Aspect { get; }

        private readonly AspectBehavior _behavior;
        private readonly Sprite _activeAspectSprite;
        private readonly Sprite _inactiveAspectSprite;
        public bool IsActive => Aspect.IsActive;

        public AspectView(
            IAspect aspect,
            AspectBehavior aspectBehavior,
            Sprite activeAspectSprite,
            Sprite inactiveAspectSprite)
        {
            Aspect = aspect;

            _behavior = aspectBehavior;
            _activeAspectSprite = activeAspectSprite;
            _inactiveAspectSprite = inactiveAspectSprite;

            _behavior.SetSprite(_activeAspectSprite);
            _behavior.SetCount(Aspect.Count);
        }

        public void SetParent(Transform parentTransform)
        {
            _behavior.transform.SetParent(parentTransform, false);
        }

        public void SetActive(bool isActive)
        {
            Aspect.IsActive = isActive;
            
            _behavior.SetSprite(isActive ? _activeAspectSprite : _inactiveAspectSprite);
        }

        public void Update()
        {
            Aspect.Update();

            _behavior.SetSprite(_activeAspectSprite);
            _behavior.SetCount(Aspect.Count);
        }

        public void Destroy()
        {
            _behavior.Destroy();
        }
    }
}