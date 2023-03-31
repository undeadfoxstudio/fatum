using System;
using UnityEngine;

namespace TableMode
{
    public interface IActionCardBehavior : ICardBehavior
    {   
        public void Init(
            IActionCard actionCard, 
            Action<Collision> OnCollisionEnter);
        
        public void Draw();
    }
}