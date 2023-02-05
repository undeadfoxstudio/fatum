using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TableMode
{
    public class TableSlotsDataInspector : EditorWindow
    {
        [CustomEditor(typeof(TableSlotsConfig))]
        public class TableSlotsDataEditor : Editor
        {
            private TableSlotsConfig _SO;

            private readonly Dictionary<Vector2Int, Vector3> _slots = new Dictionary<Vector2Int, Vector3>();
            private readonly Dictionary<Vector2Int, Vector3[]> _slotRects = new Dictionary<Vector2Int, Vector3[]>();
            private Vector3[] _tableRect = new Vector3[4];

            private ColorStatic<TableSlotsDataEditor> _slotColorStatic;
            private ColorStatic<TableSlotsDataEditor> _colliderColorStatic;

            private void OnEnable()
            {
                _SO = (TableSlotsConfig)target;
                _SO.OnChange += OnChange;

                _slotColorStatic = new ColorStatic<TableSlotsDataEditor>(nameof(_slotColorStatic));
                _colliderColorStatic = new ColorStatic<TableSlotsDataEditor>(nameof(_colliderColorStatic));

                RecalculateSlots();

                SceneView.duringSceneGui += OnSceneGUI;
            }

            private void OnDisable()
            {
                _SO.OnChange -= OnChange;

                SceneView.duringSceneGui -= OnSceneGUI;
            }

            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                _slotColorStatic.color = EditorGUILayout.ColorField("SlotColor", _slotColorStatic.color);
                _colliderColorStatic.color = EditorGUILayout.ColorField("ColliderColor", _colliderColorStatic.color);
            }

            private void OnSceneGUI(SceneView sceneView)
            {
                foreach (var rect in _slotRects)
                    Handles.DrawSolidRectangleWithOutline(
                        rect.Value,
                        _slotColorStatic.color,
                        Color.black);

                Handles.DrawSolidRectangleWithOutline(
                    _tableRect,
                    _colliderColorStatic.color,
                    Color.black);
            }

            private void OnChange()
            {
                RecalculateSlots();
            }

            private void RecalculateSlots()
            {
                _slots.Clear();

                var offsetX = GetSlotsCenterOffset(_SO.CountSlotX, _SO.SlotPaddingWidth, _SO.SlotWidth);
                var offsetY = GetSlotsCenterOffset(_SO.CountSlotY, _SO.SlotPaddingHeight, _SO.SlotHeight);

                for (var x = 0; x < _SO.CountSlotX; x++)
                for (var y = 0; y < _SO.CountSlotY; y++)
                {
                    var slotPosition = _SO.TableCenterPosition;

                    slotPosition.x += x * _SO.SlotWidth + _SO.SlotPaddingWidth * x - offsetX;
                    slotPosition.z += y * _SO.SlotHeight + _SO.SlotPaddingHeight * y - offsetY;

                    _slots.Add(new Vector2Int(x, y), slotPosition);
                }

                RecalculateSlotRects();
            }

            private void RecalculateSlotRects()
            {
                _slotRects.Clear();

                foreach (var slot in _slots)
                {
                    var rOffsetX = _SO.SlotWidth / 2;
                    var rOffsetY = _SO.SlotHeight / 2;

                    var sCenter = new Vector3(
                        slot.Value.x,
                        _SO.TableCenterPosition.y,
                        slot.Value.z);

                    var rectVectors = new[]
                    {
                        new Vector3(sCenter.x - rOffsetX, sCenter.y, sCenter.z - rOffsetY),
                        new Vector3(sCenter.x - rOffsetX, sCenter.y, sCenter.z + rOffsetY),
                        new Vector3(sCenter.x + rOffsetX, sCenter.y, sCenter.z + rOffsetY),
                        new Vector3(sCenter.x + rOffsetX, sCenter.y, sCenter.z - rOffsetY)
                    };

                    _slotRects[slot.Key] = rectVectors;
                }

                var tableRectCenter = new Vector2(_SO.TableCenterPosition.x, _SO.TableCenterPosition.z);

                _tableRect = new[]
                {
                    new Vector3(
                        tableRectCenter.x + _SO.TableZone.x / 2,
                        _SO.TableZone.z,
                        tableRectCenter.y + _SO.TableZone.y / 2),
                    new Vector3(
                        tableRectCenter.x + _SO.TableZone.x / 2,
                        _SO.TableZone.z,
                        tableRectCenter.y - _SO.TableZone.y / 2),
                    new Vector3(
                        tableRectCenter.x - _SO.TableZone.x / 2,
                        _SO.TableZone.z,
                        tableRectCenter.y - _SO.TableZone.y / 2),
                    new Vector3(
                        tableRectCenter.x - _SO.TableZone.x / 2,
                        _SO.TableZone.z,
                        tableRectCenter.y + _SO.TableZone.y / 2)
                };
            }

            private float GetSlotsCenterOffset(int count, float padding, float slotSize) =>
                slotSize * (count - 1) / 2 + padding * (count - 1) / 2;
        }
    }
}