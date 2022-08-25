using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Grid3D))]
public class Grid3DEditor : Editor
{
        public override void OnInspectorGUI()
        {
                Grid3D grid = (Grid3D)target;

                DrawDefaultInspector();

                if (GUILayout.Button("Generate Modules"))
                {
                        GameObject.DestroyImmediate(GameObject.Find("Debug").gameObject);
                        new GameObject("Debug");

                        grid.Generate3DMatrix();
                }
                //add everthing the button would do.
        }
}