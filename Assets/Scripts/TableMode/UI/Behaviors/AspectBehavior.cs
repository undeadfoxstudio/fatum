using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AspectBehavior : MonoBehaviour
{
    [SerializeField] private Image AspectSprite;
    [SerializeField] private TextMeshProUGUI AspectCount;

    public void SetSprite(Sprite sprite)
    {
        AspectSprite.sprite = sprite;
    }

    public void SetCount(int count)
    {
        if (count == 0)
            AspectCount.enabled = false;
        else
        {
            AspectCount.enabled = true;
            AspectCount.text = count.ToString();
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
