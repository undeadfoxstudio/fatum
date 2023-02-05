using UnityEditor;
using UnityEngine;

internal static class EditorPrefsExtensions
{
    public static void SetColor(string key, Color color)
    {
        EditorPrefs.SetFloat(key+"_r", color.r);
        EditorPrefs.SetFloat(key+"_g", color.g);
        EditorPrefs.SetFloat(key+"_b", color.b);
        EditorPrefs.SetFloat(key+"_a", color.a);
    }

    public static Color GetColor(string key)
    {
        return new Color(
            EditorPrefs.GetFloat(key + "_r"),
            EditorPrefs.GetFloat(key + "_g"),
            EditorPrefs.GetFloat(key + "_b"),
            EditorPrefs.GetFloat(key + "_a"));
    }
}

