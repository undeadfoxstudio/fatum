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

        [Inject]
        private void Init(
            ITableProvider tableProvider,
            ICardViewFactory cardViewFactory,
            ITableController tableController,
            IHandController handController)
        {
            _tableProvider = tableProvider;
            _tableController = tableController;
            _cardViewFactory = cardViewFactory;
            _handController = handController;
        }

        private void Start()
        {
            for (var j = 0; j < 1; j++)
            {
                var card = _cardViewFactory.CreateEntityCard("right_leg");
                var slots = _tableProvider.Positions;
                var randomVector = slots.ElementAt(Random.Range(0, slots.Count)).Key;

                card.MoveImmediately(_tableController.SetCardNearby(randomVector, card));
            }
            
            for (var j = 0; j < 1; j++)
            {
                var card = _cardViewFactory.CreateEntityCard("destiny");
                var slots = _tableProvider.Positions;
                var randomVector = slots.ElementAt(Random.Range(0, slots.Count)).Key;

                card.MoveImmediately(_tableController.SetCardNearby(randomVector, card));
            }

            var firstCardIds = new List<string> { "determination" };

            for (var i = 0; i < 5; i++)
            {
                var card = _cardViewFactory.CreateActionCard(
                    firstCardIds.ElementAt(Random.Range(0, firstCardIds.Count)));

                _handController.TakeCard(card);
            }
        }
    }
}