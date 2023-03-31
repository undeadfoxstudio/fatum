using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAspect : MonoBehaviour
{
    [SerializeField] private Image _aspectImage;
    [SerializeField] private TextMeshProUGUI _caption;
    [SerializeField] private TextMeshProUGUI _time;
    [SerializeField] private TextMeshProUGUI _tempAspectDescription;
    [SerializeField] private TextMeshProUGUI InactiveAspectText;
    
    public void SetCaption(string caption, bool isActive)
    {
        _caption.text = caption;
    } 

    public void SetSprite(Sprite sprite, bool isActive = false)
    {
        _aspectImage.sprite = sprite;

        InactiveAspectText.enabled = !isActive;
    } 

    public void SetTime(int time)
    {
        _tempAspectDescription.enabled = time != 0;
        if (time == 0)
            _time.enabled = false;
        else
            _time.text = time.ToString();
    }
}
