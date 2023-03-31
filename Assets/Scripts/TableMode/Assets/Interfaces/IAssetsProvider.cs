using UnityEngine;

namespace TableMode
{
    public interface IAssetsProvider
    {
        Sprite GetCardFaceSprite(string name);
        SpritePack GetCardSpritePack(string name);
        Sprite GetActiveAspectSprite(string name);
        Sprite GetInactiveAspectSprite(string name);
        Sprite GetAntiAspectSprite(string name);
    }
}
