using System;
using UnityEngine;

public class UIPrefab : MonoBehaviour, IUIBehavior
{
    public event Action OnNextStep;
    
    public void OnNextStepButtonClick()
    {
        OnNextStep?.Invoke();
    }
}