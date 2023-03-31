using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public AudioSource AudioSource;
    public GameObject Screen;

    public void OnStartButton()
    {
        AudioSource.Play();
        Screen.SetActive(false);
    }
}
