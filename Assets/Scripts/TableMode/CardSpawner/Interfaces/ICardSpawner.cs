using UnityEngine;

namespace TableMode
{
    public interface ICardSpawner
    {
        void SpawnActionCardById(string cardId, Vector2Int position);
        void SpawnActionCardFromGroup(string groupId, int count);
        void SpawnActionCardDefault(); 
        void SpawnEntity(string entitiesId, Vector2Int? oldPosition = null);
        void SpawnEntities(string groupName, int count, Vector2Int? position = null);
    }
}