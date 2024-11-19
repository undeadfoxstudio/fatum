using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TableMode
{
    public class TableProvider : ITableProvider
    {
        public BoxCollider Collider { get; }
        private readonly TableSlotsConfig _tableSlotsConfig;

        public Dictionary<Vector2Int, Vector3> Positions { get; } = new ();

        public TableProvider(TableSlotsConfig tableSlotsConfig, BoxCollider tableCollider)
        {
            _tableSlotsConfig = tableSlotsConfig;

            Collider = tableCollider;
            Collider.center = _tableSlotsConfig.TableCenterPosition;
            Collider.size = new Vector3(
                _tableSlotsConfig.TableZone.y,
                0,
                _tableSlotsConfig.TableZone.x);

            GenerateSlots()
                .ToList()
                .ForEach(x => Positions.Add(x.Key, x.Value));
        }

        public Vector3 GetSlotPosition(Vector2Int slotPosition)
        {
            if (Positions.ContainsKey(slotPosition))
                return Positions[slotPosition];

            throw new Exception("Trying to get access to null slot: " + slotPosition);
        }

        private Dictionary<Vector2Int, Vector3> GenerateSlots()
        {
            var newSlots = new Dictionary<Vector2Int, Vector3>();

            var offsetX = GetSlotsCenterOffset(
                _tableSlotsConfig.CountSlotX,
                _tableSlotsConfig.SlotPaddingWidth,
                _tableSlotsConfig.SlotWidth);

            var offsetY = GetSlotsCenterOffset(
                _tableSlotsConfig.CountSlotY,
                _tableSlotsConfig.SlotPaddingHeight,
                _tableSlotsConfig.SlotHeight);

            for (var x = 0; x < _tableSlotsConfig.CountSlotX; x++)
                for (var y = 0; y < _tableSlotsConfig.CountSlotY; y++)
                {
                    var newPosition = _tableSlotsConfig.TableCenterPosition;

                    newPosition.x += x * _tableSlotsConfig.SlotWidth + _tableSlotsConfig.SlotPaddingWidth * x - offsetX;
                    newPosition.z += y * _tableSlotsConfig.SlotHeight + _tableSlotsConfig.SlotPaddingHeight * y - offsetY;

                    newSlots.Add(new Vector2Int(x, y), newPosition);
                }

            return newSlots;
        }

        private float GetSlotsCenterOffset(int count, float padding, float slotSize) 
            => slotSize * (count - 1) / 2 + padding * (count - 1) / 2;
    }
}