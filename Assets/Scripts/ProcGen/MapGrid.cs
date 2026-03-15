using System.Text;
using UnityEngine;

public class MapGrid
{
    private int width, length;
    private Cell[,] cellGrid;

    public int Width { get => width; }
    public int Length { get => length; }

    public MapGrid(int width, int length)
    {
        this.width = width;
        this.length = length;
        CreateGrid();
    }

    private void CreateGrid()
    {
        cellGrid = new Cell[width, length];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                cellGrid[x, z] = new Cell(x, z);
            }
        }
    }

    public void SetCell(int x, int z, CellObjectType type, bool isTake = false)
    {
        cellGrid[x, z].Type = type;
        cellGrid[x, z].IsTaken = isTake;
    }

    public void SetCell(float x, float z, CellObjectType type, bool isTake = false)
    {
        SetCell((int)x, (int)z, type, isTake);
    }

    public bool IsCellTaken(int x, int z)
    {
        return cellGrid[x, z].IsTaken;
    }

    public bool IsCellTaken(float x, float z)
    {
        return IsCellTaken((int)x, (int)z);
    }


    public Cell GetCell(int x, int z)
    {
        if (!isCellValid(x, z))
            return null;

        return cellGrid[x, z];
    }

    public Cell GetCell(float x, float z)
    {
        if (!isCellValid(x, z))
            return null;

        return GetCell((int)x, (int)z);
    }

    public bool isCellValid(float x, float z)
    {
        if (x >= width || x <0 || z >= length || z < 0)
        {
            return false;
        }

        return true;
    }


    public int CalculateIndexFromCoordinates(int x, int z)
    {
        return x +z* width;
    }

    public int CalculateIndexFromCoordinates(float x, float z)
    {
        return CalculateIndexFromCoordinates((int)x, (int)z);
    }

    public void CheckCoordinates()
    {
        for (int i = 0; i < cellGrid.GetLength(0); i++)
        {
            StringBuilder b = new StringBuilder();
            for (int j = 0; j < cellGrid.GetLength(1); j++)
            {
                b.Append(j+ "," + i + " ");
                Debug.Log(b.ToString());
            }
        }
    }

}
