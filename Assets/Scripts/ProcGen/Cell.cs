using UnityEngine;

public enum CellObjectType
{
    Empty,
    Road,
    Obstacle,
    Start,
    Exit
}
public class Cell
{
    private int x, z;
    private bool isTaken;
    private CellObjectType type;

    public int X { get => x; }
    public int Z { get => z; }
    public bool IsTaken { get => isTaken; set => isTaken = value; }
    public CellObjectType Type { get => type; set => type = value; }

    public Cell(int x, int z)
    {
        this.x = x;
        this.z = z;
        this.isTaken = false;
        this.type = CellObjectType.Empty;   
    }
}
