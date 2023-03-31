using System;
using System.Collections.Generic;

public interface IUIBehavior
{
    void SetCardCaption(string caption);
    void SetCardDescription(string caption);
    void SetAspects(IList<UIAspect> aspects);
    void ShowInspector();
    void HideInspector();
    void PushLog(string log);
    void SetNextStepLog(string log);
    void ShowEndScreen();
    event  Action OnNextStepClick;
    event  Action OnContactAsButton;
    event  Action OnContactAsTgButton;
    event  Action OnContactAsSkypeButton;
    event Action OnSiteButton;
}