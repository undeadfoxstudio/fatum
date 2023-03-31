using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TableMode
{
    public class ActionCardView : IActionCardView, IDisposable
    {
        private readonly IActionCard _actionCard;
        private readonly IMergeStorage _mergeStorage;
        private readonly IActionCardBehavior _behavior;
        private readonly ITextureGenerator _textureGenerator;
        private Vector3 _offsetDragPosition;
        private Vector3 _firstDragPosition;
        private bool IsHovered;

        public string Id => _actionCard.Id;
        public string Name => _actionCard.Name;
        public string Description => _actionCard.Description;
        public Vector3 Position => _behavior.BehaviorTransform.position;

        public Collider Collider => _behavior.HoverCollider;
        public IEnumerable<IAspect> Aspects => _aspectViews.Select(a => a.Aspect);
        public IEnumerable<IAspect> AntiAspects => _antiAspectViews.Select(a=> a.Aspect);
        public Vector3 OffsetMove => _offsetDragPosition;

        public event Action<IActionCardView> OnDestroy;

        private readonly List<IAspectView> _aspectViews;
        private readonly List<IAspectView> _antiAspectViews;
        private readonly Dictionary<IAspectView, int> _currentAspects = new Dictionary<IAspectView, int>();

        public ActionCardView(
            IActionCardBehavior actionCardBehavior,
            ITextureGenerator textureGenerator,
            IActionCard actionCard,
            IMergeStorage mergeStorage,
            IEnumerable<IAspectView> aspects,
            IEnumerable<IAspectView> antiAspects)
        {
            _actionCard = actionCard;
            _mergeStorage = mergeStorage;
            _behavior = actionCardBehavior;
            _textureGenerator = textureGenerator;
            _behavior.Init(
                _actionCard, 
                OnCollisionEnter);

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

            return emptySlots.ElementAt(Random.Range(0, emptySlots.Count));
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
        
        private void UpdateGradient()
        {
            var colors = new List<Color>();

            foreach (var aspectView in _aspectViews)
            {
                if (aspectView.Aspect.GradientColor != Color.gray)
                    colors.Add(aspectView.Aspect.GradientColor);
            }

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

        public void MoveImmediately(Vector3 point, bool local = false)
        {
            if (local)
                _behavior.BehaviorTransform.position = point - _offsetDragPosition;
            else
                _behavior.BehaviorTransform.localPosition = point;
        }

        public void NextStep()
        {
            _actionCard.NextStep();

            foreach (var aspectView in _aspectViews)
                aspectView.Update();

            foreach (var antiAspectView in _antiAspectViews)
                antiAspectView.Update();

            _behavior.Draw();
        }
        
        public void AddAspect(IAspectView aspect)
        {
            PlaceAspect(aspect);

            _behavior.Draw();
            
            UpdateGradient();
        }

        public void RemoveAspect(string aspectId)
        {
            var removedAspect = _aspectViews.First(a => a.Aspect.Id == aspectId);

            removedAspect.Update();
            _actionCard.Aspects.Remove(
                _actionCard.Aspects.FirstOrDefault(a => a.Id == aspectId)
            );
            
            
            UpdateGradient();
        }

        public void RemoveAntiAspect(string aspectId)
        {
            var removedAntiAspect = _antiAspectViews.First(a => a.Aspect.Id == aspectId);

            removedAntiAspect.Update();

            _actionCard.AntiAspects.Remove(
                _actionCard.AntiAspects.FirstOrDefault(a => a.Id == aspectId)
            );
            
            
            UpdateGradient();
        }

        public void Update()
        {
            _actionCard.NextStep();

            foreach (var aspectView in _aspectViews)
                aspectView.Update();

            foreach (var antiAspectView in _antiAspectViews)
                antiAspectView.Update();

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