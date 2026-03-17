using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridPlacer))]
public class GridPlacerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridPlacer myScript = (GridPlacer)target;

        GUILayout.Space(10); 

        if (GUILayout.Button("Generate Grid"))
        {
            myScript.GenerateGrid();
        }
    }
}