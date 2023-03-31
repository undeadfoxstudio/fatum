using UnityEngine;

public interface ITextureGenerator
{
    Texture2D GenerateGradientPattern(Color color1, Color color2, Color color3, Color color4);
}