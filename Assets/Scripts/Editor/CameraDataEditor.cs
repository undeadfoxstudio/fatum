using UnityEditor;
using UnityEngine;

namespace TableMode
{
    public class CameraDataInspector : EditorWindow
    {
        [CustomEditor(typeof(CameraConfig))]
        public class CameraDataEditor : Editor
        {
            private CameraConfig _SO;

            private void OnEnable()
            {
                _SO = (CameraConfig)target;
                _SO.OnChange += OnChange;

                SceneView.duringSceneGui += OnSceneGUI;
            }

            private void OnDisable()
            {
                _SO.OnChange -= OnChange;

                SceneView.duringSceneGui -= OnSceneGUI;
            }

            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();
            }

            private void OnSceneGUI(SceneView sceneView)
            {
                var rectVectors = new[]
                {
                    _SO.LeftLimitPoint,
                    _SO.RightLimitPoint,
                    _SO.ForwardLimitPoint,
                    _SO.BackwardLimitPoint
                };

                Handles.DrawSolidRectangleWithOutline(
                    rectVectors,
                    Color.white,
                    Color.black);
            }

            private void OnChange() {}
        }
    }
}