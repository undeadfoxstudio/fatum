using UnityEngine;

public class ColorStatic<T>
{
    private readonly string key;
    private readonly T typeName;
    private Color colorEditor;

    public Color color
    {
        get => EditorPrefsExtensions.GetColor(nameof(typeName) + key);
        set
        {
            if (value == colorEditor) return;

            EditorPrefsExtensions.SetColor(nameof(typeName) + key, value);
            colorEditor = value;
        }
    }

    public ColorStatic(string key)
    {
        this.key = key;
        colorEditor = color;
    } 
}