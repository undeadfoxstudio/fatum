using UnityEngine;

public class NextStepClick : MonoBehaviour
{
    public Animation _animation;
    public void OnNextStepButtonClick() => _animation.Play();
}
