using System;

namespace TableMode
{
    public interface IMoveActionCardsController
    {
        event Action OnActionCardDrop;
    }
}