using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = System.Random;

namespace TableMode
{
    public class EntityCardView : IEntityCardView, IDisposable
    {
        public event Action<IEntityCardView> OnDestroy;
        public Collider Collider => _behavior.HoverCollider;
        public string Id => _entityCard.Id;
        public string Name => _entityCard.Name;
        public string Description => _entityCard.Description;
        public Vector3 Position => _behavior.BehaviorTransform.position;
        public IEnumerable<IAspect> Aspects => _aspectViews.Select(a => a.Aspect);
        public IEnumerable<IAspect> AntiAspects => _antiAspectViews.Select(a => a.Aspect);
        public Vector3 OffsetMove => _offsetDragPosition;

        private readonly IMergeStorage _mergeStorage;
        private readonly ITextureGenerator _textureGenerator;
        private readonly IEntityCard _entityCard; 
        private readonly IEntityCardBehavior _behavior;
        private bool IsHovered;
        private Vector3 _firstDragPosition;
        private Vector3 _offsetDragPosition;

        private readonly List<IAspectView> _aspectViews;
        private readonly List<IAspectView> _antiAspectViews;
        private readonly Dictionary<IAspectView, int> _currentAspects = new Dictionary<IAspectView, int>();
        
        public EntityCardView(
            IMergeStorage mergeStorage,
            ITextureGenerator textureGenerator,
            IEntityCardBehavior entityCardBehavior,
            IEntityCard entityCard,
            IEnumerable<IAspectView> aspects,
            IEnumerable<IAspectView> antiAspects)
        {
            _mergeStorage = mergeStorage;
            _textureGenerator = textureGenerator;
            _entityCard = entityCard;

            _behavior = entityCardBehavior;
            _behavior.Init(
                _entityCard, 
                OnCollisionEnter, 
                OnCollisionExit);

            _aspectViews = aspects.ToList();
            _antiAspectViews = antiAspects.ToList();

            foreach (var aspectView in _aspectViews)
                PlaceAspect(aspectView);

            foreach (var antiAspectView in _antiAspectViews)
                PlaceAspect(antiAspectView);

            UpdateGradient();
        }

        private void PlaceAspect(IAspectView aspectView)
        {
            var position = GetRandomEmptySlotAspect();

            _currentAspects.Add(aspectView, position);

            aspectView.SetParent(_behavior.slots.ElementAt(position).transform);
        }

        private int GetRandomEmptySlotAspect()
        {
            var i = 0; var slots = _behavior.slots.Select(s => i++).ToList();
            var emptySlots = slots
                .Except(_currentAspects.Values)
                .ToList();

            if (!emptySlots.Any()) throw new Exception("Too much aspects");

            return emptySlots.ElementAt(new Random().Next(0,emptySlots.Count));
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
        
        public void AddAspect(IAspectView aspectView)
        {
            if (_aspectViews.FirstOrDefault(a => a.Aspect.Id == aspectView.Aspect.Id) != null)
                RemoveAspect(aspectView.Aspect.Id);

            _aspectViews.Add(aspectView);

            PlaceAspect(aspectView);
            
            _behavior.Draw();
            
            UpdateGradient();
        }

        //TODO
        public void UpdateGradient()
        {
            var colors = _aspectViews
                .Select(aspectView => aspectView.Aspect.IsActive ? aspectView.Aspect.GradientColor : Color.gray)
                .ToList();

            if (colors.Count == 0) colors.Add(Color.gray);
            
            Texture2D texture = null;

            switch (colors.Count)
            {
                case 1:
                    texture = _textureGenerator.GenerateGradientPattern(
                        colors.ElementAt(0),
                        colors.ElementAt(0),
                        colors.ElementAt(0),
                        colors.ElementAt(0));
                    break;
                case 2:
                    texture = _textureGenerator.GenerateGradientPattern(
                        colors.ElementAt(0),
                        colors.ElementAt(0),
                        colors.ElementAt(1),
                        colors.ElementAt(1));
                    break;
                case 3:
                    texture = _textureGenerator.GenerateGradientPattern(
                        colors.ElementAt(0),
                        colors.ElementAt(1),
                        colors.ElementAt(2),
                        colors.ElementAt(2));
                    break;
            }

            if (colors.Count >= 4)
            {
                texture = _textureGenerator.GenerateGradientPattern(
                    colors.ElementAt(0),
                    colors.ElementAt(1),
                    colors.ElementAt(2),
                    colors.ElementAt(3));
            }
            
            _behavior.SetGradient(texture);
        }

        public void DisableAspect(string aspectId)
        {
            _aspectViews.First(a => a.Aspect.Id == aspectId).SetActive(false);

            _behavior.Draw();
        }

        public void RemoveAspect(string aspectId)
        {
            var removedAspect = _aspectViews.First(a => a.Aspect.Id == aspectId);

            _currentAspects.Remove(removedAspect);
            _aspectViews.Remove(removedAspect);

            removedAspect.Destroy();

            _behavior.Draw();
            
            UpdateGradient();
        }

        public void RemoveAntiAspect(string aspectId)
        {
            var removedAspect = _antiAspectViews.First(a => a.Aspect.Id == aspectId);

            _antiAspectViews.Remove(removedAspect);
            removedAspect.Destroy();

            _behavior.Draw();
            
            UpdateGradient();
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

            foreach (var aspectView in _aspectViews)
                aspectView.Update();

            foreach (var antiAspectView in _antiAspectViews)
                antiAspectView.Update();

            UpdateGradient();

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