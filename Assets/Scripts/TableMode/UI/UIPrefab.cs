using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPrefab : MonoBehaviour, IUIBehavior
{
    [SerializeField] private TextMeshProUGUI _nextStepCaption;
    [SerializeField] private UICardInspector _uiCardInspector;
    [SerializeField] private ScrollRect _scrollRectLog;
    [SerializeField] private TextMeshProUGUI _log;
    [SerializeField] private CanvasGroup _nextStepScreen;
    [SerializeField] private GameObject _nextStepScreenObject;
    [SerializeField] private TextMeshProUGUI _nextStepScreenText;
    [SerializeField] private TextMeshProUGUI _helperScreenText;
    [SerializeField] private GameObject _endScreen;
    
    private int helperCount;
    
    public event Action OnNextStepClick;
    public event Action OnContactAsButton;
    public event Action OnContactAsTgButton;
    public event Action OnContactAsSkypeButton;
    public event Action OnSiteButton;
    public event Action OnRepeatButton;

    public void OnContactAsTgButtonClick()
    {
        OnContactAsTgButton?.Invoke();
    }
    
    public void OnContactAsButtonClick()
    {
        OnContactAsButton?.Invoke();
    }

    public void OnContactAsSkypeButtonClick()
    {
        OnContactAsSkypeButton?.Invoke();
    }
    
    public void ShowEndScreen()
    {
        _endScreen.SetActive(true);
    }

    public void OnRepeatButtonClick()
    {
        _endScreen.SetActive(false);

        OnRepeatButton?.Invoke();
    }

    public void OnSiteButtonClick()
    {
        OnSiteButton?.Invoke();
    }

    private readonly List<string> _helperTexts = new List<string>
    {
        "The same <color=blue>aspects</color> of the cards allow you to combine the <color=green>card</color> from your hand with the <color=red>card</color> on the table.",
        "You can use each <color=blue>aspect</color> only once per turn.",
        "<color=blue>Temporal aspects</color> refresh the card when their timer runs out.",
        "Something happens when the <color=blue>temporal aspect</color> on the card runs out.",
        "Aspects such as <color=blue>Focus</color> and <color=blue>Time</color> remove the card when they expire.",
        "<color=purple>Anti-aspects</color> prohibit combining cards.",
        "If there are too many cards like <color=red>Rat</color> or <color=red>Madness</color> on the table, you will lose.",
        "<color=blue>Aspects</color> on the <color=red>card</color> turn gray for one turn after being combined. The bright colors disappear from the card.",
        "The more <color=blue>aspects</color> match at the time of combination, the more effective the result."
    };

    private void Awake()
    {
        _nextStepScreenObject.SetActive(false);
    }

    public void SetAspects(IList<UIAspect> aspects)
    {
        _uiCardInspector.SetAspects(aspects);
    }

    public void ShowInspector()
    {
        _uiCardInspector.ShowInspector();
    }

    public void HideInspector()
    {
        _uiCardInspector.HideInspector();
    }

    public void PushLog(string log)
    {
        _log.text += log;
        _scrollRectLog.gameObject.SetActive(false);
        _scrollRectLog.gameObject.SetActive(true);
        _scrollRectLog.verticalNormalizedPosition = 0f;
    }

    public void ClearLog()
    {
        _log.text = "";
    }

    public void SetNextStepLog(string log)
    {
        _nextStepCaption.text = log;
        _nextStepScreenText.text = log;
    }
    
    public void SetCardCaption(string caption)
    {
        _uiCardInspector.SetCaption(caption);
    }
    
    public void SetCardDescription(string description)
    {
        _uiCardInspector.SetDescription(description);
    }

    public void OnNextStepButtonClick()
    {
        _nextStepScreenObject.SetActive(true);
        
        if (helperCount == _helperTexts.Count)
            helperCount = 0;
        
        StartCoroutine(FadeOut(0.01f));
    }
    
    private IEnumerator FadeOut(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            
            if (_nextStepScreen.alpha < 1f)
                _nextStepScreen.alpha += 0.01f;
            else
            {
                StartCoroutine(FadeIn(0.01f));

                yield break;
            }
        }
    }
    
    private IEnumerator FadeIn(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            
            if (_nextStepScreen.alpha > 0)
                _nextStepScreen.alpha -= 0.01f;
            else
            {
                _nextStepScreenObject.SetActive(false);
                _helperScreenText.text = _helperTexts[helperCount];
                helperCount++;
                
                OnNextStepClick?.Invoke();

                yield break;
            }
        }
    }
}