using System.Collections;
using TMPro;
using UnityEngine;

public class AnimateAspectCount : MonoBehaviour
{
    public TextMeshProUGUI count;

    private float alpha = 1f;
    private Color currentAlphaColor;
    private bool IsFadeIn;

    void Start()
    {
        currentAlphaColor = count.color;

        StartCoroutine(WaitAndPrint(0.01f));
    }

    private IEnumerator WaitAndPrint(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);

            if (alpha <= 0) IsFadeIn = false;
            if (alpha >= 1f) IsFadeIn = true;

            alpha += IsFadeIn ? -0.025f : 0.025f;

            currentAlphaColor = new Color(
                currentAlphaColor.r,
                currentAlphaColor.g,
                currentAlphaColor.b, 
                alpha);

            count.color = currentAlphaColor;
        }
    }
}
