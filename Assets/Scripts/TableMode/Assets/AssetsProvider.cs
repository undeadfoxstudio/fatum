using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace TableMode
{
    public class AssetsProvider : IAssetsProvider
    {
        private readonly AssetsConfig _assetsConfig;

        public AssetsProvider(AssetsConfig assetsConfig)
        {
            _assetsConfig = assetsConfig;
        }

        public Sprite GetCardFaceSprite(string name) =>
            GetSprite(Path.Combine(_assetsConfig.CardFaceAssetsDirectory, name));

        public SpritePack GetCardSpritePack(string name)
        {
            var blackMask = GetSprite(Path.Combine(
                _assetsConfig.CardLayoutAssetsDirectory, 
                name + _assetsConfig.CardLayoutBlackPostfix));
            var whiteLines = GetSprite(Path.Combine(
                _assetsConfig.CardLayoutAssetsDirectory, 
                name + _assetsConfig.CardLayoutWhitePostfix));

            var spritePack = new SpritePack(
                blackMask,
                whiteLines
            );

            return spritePack;
        }

        private Sprite GetSprite(string path)
        {
            var sprite = Resources.Load<Sprite>(path);
            if (sprite == null) throw new Exception("Cant find sprite with path: " + path);

            return sprite;
        }

        public Sprite GetActiveAspectSprite(string name)
        {
            var path = Path.Combine(_assetsConfig.AspectAssetsDirectory, name);
            var sprite = Resources.Load<Sprite>(path);

            return sprite != null ? sprite : _assetsConfig.DefaultAspect;
        }

        public Sprite GetInactiveAspectSprite(string name)
        {
            var path = Path.Combine(
                _assetsConfig.AspectAssetsDirectory, 
                name + _assetsConfig.InactiveAspectNamePostfix);
            var sprite = Resources.Load<Sprite>(path);

            return sprite != null ? sprite : _assetsConfig.DefaultDisabledAspect;
        }

        public Sprite GetAntiAspectSprite(string name)
        {
            var path = Path.Combine(
                _assetsConfig.AspectAssetsDirectory, 
                name + _assetsConfig.InactiveAspectNamePostfix);
            var sprite = Resources.Load<Sprite>(path);

            return sprite != null ? sprite : _assetsConfig.DefaultDisabledAspect;
        }
    }
}