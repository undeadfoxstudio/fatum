using System.Collections.Generic;
using UnityEngine;

public interface ITableProvider
{
    Dictionary<Vector2Int,Vector3> Positions { get; }
    Vector3 GetSlotPosition(Vector2Int slotPosition);
    BoxCollider Collider { get; }
}