using UnityEngine;

namespace TableMode.TableMode.Generators
{
    public class TextureGenerator : ITextureGenerator
    {
        public Texture2D GenerateGradientPattern(Color color1, Color color2, Color color3, Color color4)
        {
            var texture = new Texture2D(10, 10);

            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    texture.SetPixel(i+5,j+1, color1);
                    texture.SetPixel(i+5,j+5, color2);
                    texture.SetPixel(i+1,j+1, color3);
                    texture.SetPixel(i+1,j+5, color4);
                }
            }
            
            texture.Apply();

            return texture;
        }
    }
}