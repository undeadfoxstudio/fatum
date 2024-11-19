using UnityEngine;
using Zenject;

namespace TableMode
{
    public class StartGameManager : MonoBehaviour
    {
        private ICardSpawner _cardSpawner;

        [Inject]
        private void Init(ICardSpawner cardSpawner)
        {
            _cardSpawner = cardSpawner;
        }

        private void Start()
        {
            _cardSpawner.SpawnEntity("new_street", new Vector2Int(10,3));
            _cardSpawner.SpawnEntity("new_room", new Vector2Int(12,3));
            _cardSpawner.SpawnEntity("desktop", new Vector2Int(14,3));
            _cardSpawner.SpawnEntity("lumber", new Vector2Int(12,2));
            _cardSpawner.SpawnEntity("rare_book2", new Vector2Int(12,2));
            _cardSpawner.SpawnEntity("im", new Vector2Int(9,2));

            _cardSpawner.SpawnActionCardDefault();
            _cardSpawner.SpawnActionCardDefault();
            _cardSpawner.SpawnActionCardDefault();
            _cardSpawner.SpawnActionCardDefault();
            _cardSpawner.SpawnActionCardDefault();
        }
    }
}