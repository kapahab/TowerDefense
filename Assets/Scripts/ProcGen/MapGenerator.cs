using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private MapVisualizer visualizer;
    void Start()
    {
        MapGrid grid = new MapGrid(10, 10);
        visualizer.VisualizeGrid(grid.Width, grid.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
