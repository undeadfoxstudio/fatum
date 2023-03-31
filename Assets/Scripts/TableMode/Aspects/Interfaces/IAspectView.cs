using UnityEngine;

namespace TableMode
{
    public interface IAspectView
    {
        bool IsActive { get; }
        void SetParent(Transform parentTransform);
        IAspect Aspect { get; }
        void SetActive(bool isActive);
        void Update();
        void Destroy();
    }
}