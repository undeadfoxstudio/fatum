using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace TableMode
{
    public class GameManager : MonoBehaviour
    {
        private ITableProvider _tableProvider;
        private IHandController _handController;
        private ICardViewFactory _cardViewFactory;
        private ITableController _tableController;
        
        private ICardSpawner _cardSpawner;
        
        [Inject]
        private void Init(
            ICardSpawner cardSpawner,
            ITableProvider tableProvider,
            ICardViewFactory cardViewFactory,
            ITableController tableController,
            IHandController handController)
        {
            _cardSpawner = cardSpawner;
            
            _tableProvider = tableProvider;
            _tableController = tableController;
            _cardViewFactory = cardViewFactory;
            _handController = handController;
        }

        private void Start()
        {
            _cardSpawner.SpawnEntity("hands", new Vector2Int(9,1));
            _cardSpawner.SpawnEntity("legs", new Vector2Int(11,1));
            _cardSpawner.SpawnEntity("head", new Vector2Int(13,1));
            _cardSpawner.SpawnEntity("mystreet", new Vector2Int(10,3));
            _cardSpawner.SpawnEntity("myroom", new Vector2Int(12,3));
            _cardSpawner.SpawnEntity("closet", new Vector2Int(17,3));
            _cardSpawner.SpawnEntity("book_closet", new Vector2Int(16,3));
            _cardSpawner.SpawnEntity("trash_pack", new Vector2Int(5,3));
            
            _cardSpawner.SpawnActionCardFromGroup("first_hand", 5);
        }
    }
}