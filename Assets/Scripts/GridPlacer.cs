using UnityEngine;

public class GridPlacer : MonoBehaviour
{
    public int width, length;
    public GameObject cellPrefab;

    public void GenerateGrid()
    {
        if (cellPrefab == null)
        {
            Debug.LogWarning("Please assign a Cell Prefab!");
            return;
        }

#if UNITY_EDITOR
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                GameObject cell = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(cellPrefab);

                cell.transform.position = new Vector3(transform.position.x + i, transform.position.y+0.08f, transform.position.z + j);

                cell.transform.SetParent(transform);

                UnityEditor.Undo.RegisterCreatedObjectUndo(cell, "Generate Grid");
            }
        }
#endif
    }
}