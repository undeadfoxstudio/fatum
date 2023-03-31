using UnityEngine;
using UnityEngine.Serialization;

namespace TableMode
{
    [CreateAssetMenu(fileName = "AssetsConfig", menuName = "Fatum/AssetsConfig", order = 1)]
    public class AssetsConfig : ScriptableConfig
    {
        public string CardFaceAssetsDirectory;
        public string CardLayoutAssetsDirectory;
        public string CardLayoutLinePostfix;
        public string CardLayoutBlackPostfix;
        public string CardLayoutWhitePostfix;
        public string AspectAssetsDirectory;
        public Sprite DefaultCardFace;
        public Sprite DefaultAspect;
        public Sprite DefaultDisabledAspect;
        public Sprite DefaultAntiAspect;
        public string InactiveAspectNamePostfix;
        public string AntiAspectNamePostfix;
    }
}