using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace TableMode
{
    public class UIController : IUIController
    {
        public event Action OnNextStep;

        private readonly IUIBehavior _iuiBehavior;
        private readonly UIAspect _uiAspectPrefab;
        private readonly UIAspect _uiAntiAspectPrefab;
        private readonly IAssetsProvider _assetsProvider;

        public UIController(
            IUIBehavior iuiBehavior,
            UIAspect uiAspectPrefab,
            UIAspect uiAntiAspectPrefab,
            IAssetsProvider assetsProvider)
        {
            _iuiBehavior = iuiBehavior;
            _uiAspectPrefab = uiAspectPrefab;
            _uiAntiAspectPrefab = uiAntiAspectPrefab;
            _assetsProvider = assetsProvider;

            iuiBehavior.OnNextStepClick += UiPrefabOnNextStepButtonClick;
            iuiBehavior.OnContactAsButton += IuiBehaviorOnContactAsButton;
            iuiBehavior.OnContactAsTgButton += IuiBehaviorOnContactAsTgButton;
            iuiBehavior.OnContactAsSkypeButton += IuiBehaviorOnOnContactAsSkypeButton;
            iuiBehavior.OnSiteButton += IuiBehaviorOnOnSiteButton;
        }

        private void IuiBehaviorOnOnSiteButton()
        {
            Application.OpenURL("https://undeadfox.com/");
        }

        private void IuiBehaviorOnOnContactAsSkypeButton()
        {
            Application.OpenURL("skype:live:.cid.91519c93604b7487?chat");
        }

        private void IuiBehaviorOnContactAsTgButton()
        {
            Application.OpenURL("https://t.me/alex_nebesky");
        }

        private void IuiBehaviorOnContactAsButton()
        {
            Application.OpenURL("mailto: account@undeadfox.com");
        }

        public void SetNextStepLog(string log)
        {
            _iuiBehavior.SetNextStepLog(log);
        }
        
        public void ShowEndScreen()
        {
            _iuiBehavior.ShowEndScreen();
        }

        public void ShowInspector()
        {
            _iuiBehavior.ShowInspector();
        }

        public void HideInspector()
        {
            _iuiBehavior.HideInspector();
        }

        public void PushLog(string logStep)
        {
            _iuiBehavior.PushLog(logStep);
        }

        public void PushLogMerge(string actionName, string entityName, string log)
        {
            var formattedActionName = "<color=green>" + actionName + "</color>";
            var formattedEntityName = "<color=red>" + entityName + "</color>";
            var formattedResult = log.IsEmpty() ? "Это действие ни к чему не привело." : log;  

            var formattedLog = 
                formattedActionName + " + " + 
                formattedEntityName + ": " +
                formattedResult + "\n";

            _iuiBehavior.PushLog(formattedLog);
        }

        public void SetAspects(IList<IAspect> aspectViews, bool isAntiAspect = false)
        {
            var aspects = new List<UIAspect>();

            foreach (var aspectView in aspectViews)
            {
                var aspect = isAntiAspect
                    ? UnityEngine.Object.Instantiate(_uiAntiAspectPrefab)
                    : UnityEngine.Object.Instantiate(_uiAspectPrefab);

                aspect.SetCaption(aspectView.Name, aspectView.IsActive);
                aspect.SetTime(aspectView.Count);
                aspects.Add(aspect);
                aspect.SetSprite(aspectView.IsActive 
                    ? _assetsProvider.GetActiveAspectSprite(aspectView.Asset) 
                    : _assetsProvider.GetInactiveAspectSprite(aspectView.Asset),
                    aspectView.IsActive);
            }

            _iuiBehavior.SetAspects(aspects);
        }

        public void SetCurrentCardCaption(string caption)
        {
            _iuiBehavior.SetCardCaption(caption);
        }

        public void SetCurrentCardDescription(string description)
        {
            _iuiBehavior.SetCardDescription(description);
        }

        private void UiPrefabOnNextStepButtonClick()
        {
            OnNextStep?.Invoke();
        }
    }
}