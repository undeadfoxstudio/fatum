using UnityEngine;

namespace TableMode
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Fatum/CameraConfig", order = 1)]
    public class CameraConfig : ScriptableConfig
    {
        public float MaxScrollDistance;
        public float MinScrollDistance;
        public float ScrollSpeed;
        public Vector3 LeftLimitPoint;
        public Vector3 RightLimitPoint;
        public Vector3 BackwardLimitPoint;
        public Vector3 ForwardLimitPoint;
    }
}