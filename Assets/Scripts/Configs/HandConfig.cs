using UnityEngine;

namespace TableMode
{
    [CreateAssetMenu(fileName = "HandConfig", menuName = "Fatum/HandConfig", order = 1)]
    public class HandConfig : ScriptableConfig
    {
        public Vector3 HandCenterPosition;
        public float SlotSize = 0.1f;
        public int CardCount = 5;
    }
}