using UnityEngine;

public class MapVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject groundPrefab;

    public void VisualizeGrid(int width, int length)
    {
        Vector3 pos = new Vector3(width / 2f, 0, length / 2f);

        GameObject ground = Instantiate(groundPrefab, pos, Quaternion.identity);

    }
}
