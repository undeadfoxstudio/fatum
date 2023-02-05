using UnityEngine;

namespace TableMode
{
    [CreateAssetMenu(fileName = "ContentConfig", menuName = "Fatum/ContentConfig", order = 1)]
    public class ContentConfig : ScriptableConfig
    {
        public TextAsset Aspects;
        public TextAsset ActionCards;
        public TextAsset EntityCards;
        public TextAsset MergeRules;
        public TextAsset AspectRules;
        public TextAsset AntiAspectRules;
    }
}