using System;
using System.Collections.Generic;

namespace TableMode
{
    public interface IUIController
    {
        void SetCurrentCardCaption(string caption);
        void SetCurrentCardDescription(string description);
        void SetAspects(IList<IAspect> aspectViews, bool isAntiAspect = false);
        void ShowInspector();
        void HideInspector();
        void PushLog(string logStep);
        void PushLogMerge(string actionName, string entityName, string log);
        void SetNextStepLog(string log);
        void ShowEndScreen();
        event Action<int> OnNextStep;
    }
}