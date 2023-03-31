using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TableMode
{
    public class ActionCardBehavior : MonoBehaviour, IActionCardBehavior, IDisposable
    {
        public Gradient gradient;
        [SerializeField] private TextMeshPro _caption;
        [SerializeField] private Collider _collider;
        [SerializeField] private Image _gradientImage;
        [SerializeField] private Image _blackMaskImage;
        [SerializeField] private Image _whiteLineImage;
        [SerializeField] private GameObject _hoverObject;
        [SerializeField] private List<GameObject> _aspectSlots;

        private IMergeStorage _mergeStorage;
        private Action<Collision> _onCollisionEnter;

        public List<GameObject> slots => _aspectSlots;

        public void SetSprite(SpritePack spritePack)
        {
            _whiteLineImage.sprite = spritePack.WhiteLines;
            _blackMaskImage.sprite = spritePack.BlackMask;
        }

        public Transform BehaviorTransform => transform;
        public Collider HoverCollider => _collider;
        public IActionCard ActionCard { get; private set; }

        public void Init(
            IActionCard actionCard,
            Action<Collision> onCollisionEnter)
        {
            _onCollisionEnter = onCollisionEnter;
            ActionCard = actionCard;

            Draw();
        }

        public void Draw()
        {
            _caption.text = ActionCard.Name;
        }

        private void OnCollisionEnter(Collision collision)
        {
            _onCollisionEnter?.Invoke(collision);
        }

        public void Hover()
        {
            _hoverObject.SetActive(true);
        }

        public void UnHover()
        {
            _hoverObject.SetActive(false);
        }

        public void Destroy()
        {
            Dispose();
        }

        public void SetGradient(Texture2D texture2D)
        {
            var newSprite = Sprite.Create(
                texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), 
                new Vector2(0, 0), 
                100.0f);

            _gradientImage.sprite = newSprite;
        }

        public void Dispose()
        {
            DestroyImmediate(gameObject);
        }
    }
}