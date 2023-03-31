using UnityEngine;

public struct SpritePack
{
    public Sprite BlackMask { get; }
    public Sprite WhiteLines { get; }

    public SpritePack(
        Sprite blackMask,
        Sprite whiteLines)
    {
        BlackMask = blackMask;
        WhiteLines = whiteLines;
    }
}