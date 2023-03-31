using UnityEngine;
using Zenject;

namespace TableMode
{
    public class CameraInstaller : MonoInstaller
    {
        public CameraConfig CameraConfig;
        public BoxCollider InteractableZoneCollider;
        public CameraController cameraController;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MoveCameraController>()
                .AsSingle()
                .WithArguments(InteractableZoneCollider, cameraController, CameraConfig)
                .NonLazy();
        }
    }
}