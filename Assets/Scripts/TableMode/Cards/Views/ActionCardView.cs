using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace TableMode
{
    public class ActionCardView : IActionCardView, IDisposable
    {
        private readonly IActionCard _actionCard;
        private readonly IMergeStorage _mergeStorage;
        private readonly IActionCardBehavior _behavior;
        private Vector3 _offsetDragPosition;
        private Vector3 _firstDragPosition;
        private bool IsHovered;

        public string Id => _actionCard.Id;
        public Vector3 Position => _behavior.BehaviorTransform.position;
        
        public Collider Collider => _behavior.HoverCollider;
        public IEnumerable<IAspect> Aspects => _actionCard.Aspects;
        public IEnumerable<IAspect> AntiAspects => _actionCard.AntiAspects;
        public event Action<IActionCardView> OnDestroy;

        public ActionCardView(
            IActionCardBehavior actionCardBehavior,
            IActionCard actionCard,
            IMergeStorage mergeStorage)
        {
            _actionCard = actionCard;
            _mergeStorage = mergeStorage;
            _behavior = actionCardBehavior;
            _behavior.Init(
                _actionCard, 
                OnCollisionEnter);
        }

        private void OnCollisionEnter(Collision collision)
        {
            var entityCardBehavior = collision.collider.gameObject.GetComponent<IEntityCardBehavior>();
            if (entityCardBehavior != null) _mergeStorage.SetActionCardView(this);
        }

        public void Hover(Vector3 point)
        {
            if (IsHovered == false) _behavior.Hover();

            _firstDragPosition = point;
            _offsetDragPosition = _firstDragPosition - _behavior.BehaviorTransform.position;
            
            _mergeStorage.SetActionCardView(this);
            
            IsHovered = true;
        }

        public void UnHover()
        {
            if (IsHovered) _behavior.UnHover();
            
            IsHovered = false;
        }

        public void Move(Vector3 newPosition, bool local = false)
        {
            if (local)
                _behavior.BehaviorTransform.DOLocalMove(newPosition, 0.3f, false).SetEase(Ease.OutQuad);
            else
                _behavior.BehaviorTransform.DOMove(newPosition, 0.3f, false).SetEase(Ease.OutQuad);
        }

        public void Rotate(float angle)
        {
            
        }

        public void MoveImmediately(Vector3 point, bool local = false)
        {
            if (local)
                _behavior.BehaviorTransform.position = point - _offsetDragPosition;
            else
                _behavior.BehaviorTransform.localPosition = point;
        }

        public void NextStep()
        {
            _behavior.Draw();
        }
        
        public void AddAspect(IAspect aspect)
        {
            _actionCard.Aspects.Add(aspect);
            _behavior.Draw();
        }
        
        public void RemoveAspect(string aspectId)
        {
            _actionCard.Aspects.Remove(
                _actionCard.Aspects.FirstOrDefault(a => a.Id == aspectId)
            );
        }

        public void Destroy()
        {
            _behavior.Destroy();

            Dispose();
        }

        public void Dispose()
        {
            OnDestroy?.Invoke(this);
        }
    }
}