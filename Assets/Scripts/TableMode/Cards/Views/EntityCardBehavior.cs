using System;
using UnityEngine;
using TMPro;
using Image = UnityEngine.UI.Image;

namespace TableMode
{
    public class EntityCardBehavior : MonoBehaviour, IEntityCardBehavior, IDisposable
    {
        [SerializeField] private TextMeshPro _caption;
        [SerializeField] private TextMeshPro _aspects;
        [SerializeField] private BoxCollider _hoverCollider;
        [SerializeField] private BoxCollider _mergeCollider;
        [SerializeField] private Image image;
        [SerializeField] private GameObject hoverObject;

        private Action<Collision> _onCollisionEnter;
        private Action _onCollisionExit;

        public event Action<IAspect> OnAspectOver;
        public Transform BehaviorTransform => transform;
        public Collider HoverCollider => _hoverCollider;
        public IEntityCard EntityCard { get; private set; }

        public void Init(
            IEntityCard entityCard,
            Action<Collision> OnCollisionEnter,
            Action OnCollisionExit)
        {
            EntityCard = entityCard;

            _onCollisionEnter = OnCollisionEnter;
            _onCollisionExit = OnCollisionExit;

            Draw();
        }

        public void Draw()
        {
            _caption.text = EntityCard.Name;
            _aspects.text = "";

            foreach (var aspect in EntityCard.Aspects)
            {
                _aspects.text += aspect.IsActive ? "<color=\"white\">" : "<color=#383838>";

                if (aspect.Count == 0)
                    _aspects.text += aspect.Name + "\n";
                else
                    _aspects.text += aspect.Name + " (" + aspect.Count + ")\n";
            }

            foreach (var antiAspect in EntityCard.AntiAspects)
            {
                _aspects.text += "<color=\"red\">" + antiAspect.Name;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            _onCollisionEnter?.Invoke(collision);
        }

        private void OnCollisionExit(Collision other)
        {
            _onCollisionExit?.Invoke();
        }

        public void Hover()
        {
            hoverObject.SetActive(true);
        }

        public void UnHover()
        {
            hoverObject.SetActive(false);
        }

        public void SetState(CardReady cardReady)
        {
            switch (cardReady)
            {
                case CardReady.ToMerge:
                    _mergeCollider.enabled = true;
                    _hoverCollider.enabled = false;
                    break;
                case CardReady.ToDrag:
                    _mergeCollider.enabled = false;
                    _hoverCollider.enabled = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cardReady), cardReady, null);
            }
        }

        public void Destroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            DestroyImmediate(gameObject);
        }
    }
}