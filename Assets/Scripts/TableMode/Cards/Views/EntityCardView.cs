using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace TableMode
{
    public class EntityCardView : IEntityCardView, IDisposable
    {
        public event Action<IEntityCardView> OnDestroy;
        public Collider Collider => _behavior.HoverCollider;
        public string Id => _entityCard.Id;
        public Vector3 Position => _behavior.BehaviorTransform.position;
        public IEnumerable<IAspect> Aspects => _entityCard.Aspects;
        public IEnumerable<IAspect> AntiAspects => _entityCard.AntiAspects;

        private readonly IMergeStorage _mergeStorage;
        private readonly IEntityCard _entityCard; 
        private readonly IEntityCardBehavior _behavior;
        private bool IsHovered;
        private Vector3 _firstDragPosition;
        private Vector3 _offsetDragPosition;

        public EntityCardView(
            IMergeStorage mergeStorage,
            IEntityCardBehavior entityCardBehavior,
            IEntityCard entityCard)
        {
            _mergeStorage = mergeStorage;
            _entityCard = entityCard;

            _behavior = entityCardBehavior;
            _behavior.Init(
                _entityCard, 
                OnCollisionEnter, 
                OnCollisionExit);
        }

        private void OnCollisionEnter(Collision collision)
        {
            var entityCardBehavior = collision.collider.gameObject.GetComponent<IActionCardBehavior>();
            if (entityCardBehavior != null)
            {
                _mergeStorage.SetEntityCardView(this);
                _behavior.Hover();
            }
        }

        private void OnCollisionExit()
        {
            _mergeStorage.ClearEntityCardView();
            _behavior.UnHover();
        }

        public void SwitchModeTo(CardReady mode) => _behavior.SetState(mode);
        
        public void AddAspect(IAspect aspect)
        {
            _entityCard.Aspects.Add(aspect);
            _behavior.Draw();
        }

        public void DisableAspect(string aspectId)
        {
            _entityCard.Aspects.First(a => a.Id == aspectId).IsActive = false;
            _behavior.Draw();
        }

        public void RemoveAspect(string aspectId)
        {
            _entityCard.Aspects.Remove(_entityCard.Aspects.First(a => a.Id == aspectId));
            _behavior.Draw();
        }

        public void Rotate(float angle)
        {
            throw new NotImplementedException();
        }

        public void Hover(Vector3 point)
        {
            if (IsHovered == false) _behavior.Hover();

            _firstDragPosition = point;
            _offsetDragPosition = _firstDragPosition - _behavior.BehaviorTransform.position;
            
            IsHovered = true;
        }

        public void UnHover()
        {
            if (IsHovered) _behavior.UnHover();

            IsHovered = false;
        }

        public void Move(Vector3 point, bool local = false)
        {
            _behavior.BehaviorTransform
                .DOMove(point, 0.3f, false)
                .SetEase(Ease.OutQuad);
        }

        public void MoveImmediately(Vector3 point, bool local = false)
        {
            if (local)
                _behavior.BehaviorTransform.position = point - _offsetDragPosition;
            else
                _behavior.BehaviorTransform.localPosition = point - _offsetDragPosition;
        }

        public void NextStep()
        {
            _entityCard.NextStep();
            _behavior.Draw();
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