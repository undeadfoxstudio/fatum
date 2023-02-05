using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TableMode
{
    public class ActionCardBehavior : MonoBehaviour, IActionCardBehavior, IDisposable
    {
        [SerializeField] private TextMeshPro _caption;
        [SerializeField] private TextMeshPro _aspects;
        [SerializeField] private Collider _collider;
        [SerializeField] private Image image;
        [SerializeField] private GameObject hoverObject;
        
        private IMergeStorage _mergeStorage;
        private Action<Collision> _onCollisionEnter;

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
            _aspects.text = "";

            foreach (var aspect in ActionCard.Aspects)
            {
                _aspects.text += aspect.IsActive ? "<color=\"white\">" : "<color=#383838>";

                if (aspect.Count == 0)
                    _aspects.text += aspect.Name + "\n";
                else
                    _aspects.text += aspect.Name + " (" + aspect.Count + ")\n";
            }

            foreach (var antiAspect in ActionCard.AntiAspects)
            {
                _aspects.text += "<color=\"red\">" + antiAspect.Name;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            _onCollisionEnter?.Invoke(collision);
        }

        public void Hover()
        {
            hoverObject.SetActive(true);
        }

        public void UnHover()
        {
            hoverObject.SetActive(false);
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