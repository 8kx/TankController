using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public int gridSize = 10;
    private Cell[,] grid;

    void Start()
    {
        InitializeGrid();
    }

    void InitializeGrid()
    {
        grid = new Cell[gridSize, gridSize];
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                grid[x, y] = new Cell(new Vector2(x, y));
            }
        }
    }

    public Cell GetCell(Vector2 position)
    {
        int x = Mathf.FloorToInt(position.x);
        int y = Mathf.FloorToInt(position.y);
        if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
        {
            return grid[x, y];
        }
        return null;
    }
}

public class Cell
{
    public Vector2 Position { get; private set; }
    public bool IsOccupied { get; set; }

    public Cell(Vector2 position)
    {
        Position = position;
        IsOccupied = false;
    }
}

public class Pathfinder
{
    private GridManager gridManager;

    public Pathfinder(GridManager gridManager)
    {
        this.gridManager = gridManager;
    }

    public List<Vector2> FindPath(Vector2 start, Vector2 target)
    {
        // Implement A* algorithm to find path from start to target
        // Return list of waypoints
        return new List<Vector2>();
    }
}
