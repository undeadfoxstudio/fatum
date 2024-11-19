using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace TableMode
{
    public class UIController : IUIController
    {
        public event Action<int> OnNextStep;

        private readonly IUIBehavior _iuiBehavior;
        private readonly UIAspect _uiAspectPrefab;
        private readonly UIAspect _uiAntiAspectPrefab;
        private readonly IAssetsProvider _assetsProvider;
        private readonly IHandController _handController;
        private readonly ITableController _tableController;
        private readonly ICardSpawner _cardSpawner;

        private int _step;
        
        public UIController(
            IUIBehavior iuiBehavior,
            UIAspect uiAspectPrefab,
            UIAspect uiAntiAspectPrefab,
            IAssetsProvider assetsProvider,
            IHandController handController,
            ITableController tableController,
            ICardSpawner cardSpawner)
        {
            _iuiBehavior = iuiBehavior;
            _uiAspectPrefab = uiAspectPrefab;
            _uiAntiAspectPrefab = uiAntiAspectPrefab;
            _assetsProvider = assetsProvider;
            _handController = handController;
            _tableController = tableController;
            _cardSpawner = cardSpawner;

            iuiBehavior.OnNextStepClick += UiPrefabOnNextStepButtonClick;
            iuiBehavior.OnContactAsButton += IuiBehaviorOnContactAsButton;
            iuiBehavior.OnContactAsTgButton += IuiBehaviorOnContactAsTgButton;
            iuiBehavior.OnContactAsSkypeButton += IuiBehaviorOnOnContactAsSkypeButton;
            iuiBehavior.OnSiteButton += IuiBehaviorOnOnSiteButton;
            iuiBehavior.OnRepeatButton += IuiBehaviorOnOnRepeatButton;
        }

        private void IuiBehaviorOnOnRepeatButton()
        {
            _handController.Clear();
            _tableController.Clear();
            
            _cardSpawner.SpawnEntity("new_street", new Vector2Int(10,3));
            _cardSpawner.SpawnEntity("new_room", new Vector2Int(12,3));
            _cardSpawner.SpawnEntity("desktop", new Vector2Int(14,3));
            _cardSpawner.SpawnEntity("lumber", new Vector2Int(12,2));
            _cardSpawner.SpawnEntity("rare_book2", new Vector2Int(12,2));

            _cardSpawner.SpawnActionCardDefault();
            _cardSpawner.SpawnActionCardDefault();
            _cardSpawner.SpawnActionCardDefault();
            _cardSpawner.SpawnActionCardDefault();
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
            var formattedResult = log.IsEmpty() ? "This action led to nothing." : log;  

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
            _step++;
            
            if (_step == 25)
            {
                _step = 1;
                
                OnNextStep?.Invoke(_step);
                
                _iuiBehavior.ClearLog();

                ShowEndScreen();

                return;
            }
            
            OnNextStep?.Invoke(_step);
        }
    }
}