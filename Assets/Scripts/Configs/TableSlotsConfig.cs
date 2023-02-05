using UnityEngine;

namespace TableMode
{
    [CreateAssetMenu(fileName = "SlotsConfig", menuName = "Fatum/SlotsConfig", order = 1)]
    public class TableSlotsConfig : ScriptableConfig
    {
        public Vector3 TableCenterPosition;

        public float SlotWidth = 0.1f;
        public float SlotHeight = 0.1f;
        public float SlotPaddingWidth = 0.01f;
        public float SlotPaddingHeight = 0.01f;
        public int CountSlotX = 27;
        public int CountSlotY = 6;
        public Vector3 TableZone;
    }
}
