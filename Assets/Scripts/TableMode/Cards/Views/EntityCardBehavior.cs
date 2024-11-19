using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Image = UnityEngine.UI.Image;

namespace TableMode
{
    public class EntityCardBehavior : MonoBehaviour, IEntityCardBehavior, IDisposable
    {
        [SerializeField] private TextMeshPro _caption;
        [SerializeField] private BoxCollider _hoverCollider;
        [SerializeField] private BoxCollider _mergeCollider;
        [SerializeField] private Image _gradientImage;
        [SerializeField] private Image _blackMaskImage;
        [SerializeField] private Image _whiteLineImage;
        [SerializeField] private GameObject _hoverObject;
        [SerializeField] private List<GameObject> _aspectSlots;

        private Action<Collision> _onCollisionEnter;
        private Action _onCollisionExit;

        public event Action<IAspect> OnAspectOver;
        public List<GameObject> slots => _aspectSlots;

        public void SetSprite(SpritePack spritePack)
        {
            _whiteLineImage.sprite = spritePack.WhiteLines;
            _blackMaskImage.sprite = spritePack.BlackMask;
        }

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
        }

        public void SetGradient(Texture2D texture2D)
        {
            var newSprite = Sprite.Create(
                texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), 
                new Vector2(0, 0), 
                100.0f);

            _gradientImage.sprite = newSprite;
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
            _hoverObject.SetActive(true);
        }

        public void UnHover()
        {
            _hoverObject.SetActive(false);
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