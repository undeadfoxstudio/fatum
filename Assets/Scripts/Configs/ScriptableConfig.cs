using System;
using UnityEngine;

namespace TableMode
{
    public abstract class ScriptableConfig : ScriptableObject
    {
        public event Action OnChange;
        private void OnValidate() => OnChange?.Invoke();
    }
}