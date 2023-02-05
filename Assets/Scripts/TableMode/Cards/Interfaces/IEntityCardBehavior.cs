using System;
using UnityEngine;

namespace TableMode
{
    public interface IEntityCardBehavior : ICardBehavior
    {
        //TODO
        IEntityCard EntityCard { get; }

        public void Init(
            IEntityCard entityCard,
            Action<Collision> OnCollisionEnter,
            Action OnCollisionExit);

        public void Draw();

        public void SetState(CardReady cardReady);
    }
}