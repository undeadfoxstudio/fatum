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

    public void OnSiteButtonClick()
    {
        OnSiteButton?.Invoke();
    }

    private List<string> _helperTexts = new List<string>
    {
        "Одинаковые <color=blue>аспекты</color> карт позволяют объединить <color=green>карту</color>" +
        " из руки с <color=red>картой</color> на столе.",
        "Каждый <color=blue>аспект</color> можно использовать только один раз за ход.",
        "<color=blue>Временные аспекты</color> обновляют карту когда их таймер заканчивается.",
        "Что-то происходит когда <color=blue>временный аспект</color> на карте заканчивается.",
        "Такие аспекты как <color=blue>Фокус</color> и <color=blue>Время</color> удаляют карту когда истекают.",
        "<color=purple>Антиаспекты</color> запрещают объединять карты.",
        "<color=blue>Аспекты</color> на <color=red>карте</color> становятся серыми на один ход после объединения. " +
        "С карты пропадают яркие краски.",
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