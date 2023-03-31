using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UICardInspector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _caption;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private GameObject _aspectsContainer;

    private Coroutine fadingCoroutine;
    private CanvasGroup _canvasGroup;
    
    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetCaption(string caption) => _caption.text = caption;
    public void SetDescription(string description) => _description.text = description;

    public void SetAspects(IEnumerable<UIAspect> aspects)
    {
        foreach (var uiAspect in aspects)
            uiAspect.transform.SetParent(_aspectsContainer.transform, false);
    }

    public void ShowInspector()
    {
        var oldAspects = _aspectsContainer.transform.childCount;
        
        for (var i = oldAspects - 1; i >= 0; i--)
            DestroyImmediate(_aspectsContainer.transform.GetChild(i).gameObject);
        
        if (fadingCoroutine != null) StopCoroutine(fadingCoroutine);
        fadingCoroutine = StartCoroutine(Show());
    }

    public void HideInspector()
    {
        if (fadingCoroutine != null) StopCoroutine(fadingCoroutine);
        fadingCoroutine = StartCoroutine(Hide());
    }
    
    IEnumerator Show()
    {
        for (float alpha = 0f; alpha <= 1; alpha += 0.1f)
        {
            _canvasGroup.alpha = alpha;
            yield return null;
        }
    }

    IEnumerator Hide()
    {
        for (float alpha = 1f; alpha > 0; alpha -= 0.1f)
        {
            _canvasGroup.alpha = alpha;
            yield return null;
        }
        
        _canvasGroup.alpha = 0;
    }
}
