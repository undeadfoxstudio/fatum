using UnityEditor;
using UnityEngine;

namespace TableMode
{
    [CustomEditor(typeof(TableImport))]
    public class TableImportEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var myScript = (TableImport)target;
            if (GUILayout.Button("Импорт"))
                myScript.Import();
        }
    }
}