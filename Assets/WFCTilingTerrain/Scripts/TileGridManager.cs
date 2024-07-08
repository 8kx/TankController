using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TileGridManager : MonoBehaviour
{
    public int gridSizeX = 10;
    public int gridSizeY = 10;
    public TileManager tileManager;
    public GameObject tilePrefab; // A single prefab to use for all tiles
    public int batchSize = 1; // Number of cells to process per frame during propagation

    private Tile[,] grid;
    private Dictionary<Vector2Int, List<Tile>> possibleTiles; // Possible tiles for each cell
    private Dictionary<(Tile, Tile, string), bool> compatibilityCache = new Dictionary<(Tile, Tile, string), bool>();

    void Start()
    {
        grid = new Tile[gridSizeX, gridSizeY];
        possibleTiles = new Dictionary<Vector2Int, List<Tile>>();

        // Initialize possible tiles for each cell
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                possibleTiles[new Vector2Int(x, y)] = new List<Tile>(tileManager.tiles);
            }
        }

        Debug.Log("Starting wave function collapse...");
        StartCoroutine(WaveFunctionCollapse());
    }

    IEnumerator WaveFunctionCollapse()
    {
        // Initialize the first tile without compatibility checks
        Vector2Int initialCell = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        Tile initialTile = possibleTiles[initialCell][Random.Range(0, possibleTiles[initialCell].Count)];
        grid[initialCell.x, initialCell.y] = initialTile;
        possibleTiles.Remove(initialCell);
        InstantiateTile(initialCell, initialTile);

        Debug.Log("Initial tile set. Propagating constraints...");
        yield return StartCoroutine(PropagateConstraints(initialCell));
        Debug.Log("Constraints propagated. Collapsing the rest of the grid...");

        while (!IsFullyCollapsed())
        {
            Vector2Int cell = SelectNextCell();
            CollapseCell(cell);
            if (grid[cell.x, cell.y] != null)
            {
                InstantiateTile(cell, grid[cell.x, cell.y]);
            }
            yield return StartCoroutine(PropagateConstraints(cell));
        }
    }

    bool IsFullyCollapsed()
    {
        foreach (Tile cell in grid)
        {
            if (cell == null)
                return false;
        }
        return true;
    }

Vector2Int SelectNextCell()
{
    List<Vector2Int> uncollapsedCells = new List<Vector2Int>();
    int minPossibilities = int.MaxValue;

    // Iterate over all possible tiles to find the cell(s) with the fewest possibilities
    foreach (var kvp in possibleTiles)
    {
        if (grid[kvp.Key.x, kvp.Key.y] == null)
        {
            int possibilitiesCount = kvp.Value.Count;

            if (possibilitiesCount < minPossibilities)
            {
                minPossibilities = possibilitiesCount;
                uncollapsedCells.Clear();
                uncollapsedCells.Add(kvp.Key);
            }
            else if (possibilitiesCount == minPossibilities)
            {
                uncollapsedCells.Add(kvp.Key);
            }
        }
    }

    // Randomly select one cell from those with the fewest possibilities
    if (uncollapsedCells.Count > 0)
    {
        return uncollapsedCells[Random.Range(0, uncollapsedCells.Count)];
    }

    // Return a default value (if no uncollapsed cells are found)
    return new Vector2Int(-1, -1);
}


    void CollapseCell(Vector2Int cell)
    {
        if (possibleTiles[cell].Count > 0)
        {
            Tile selectedTile = possibleTiles[cell][Random.Range(0, possibleTiles[cell].Count)];
            grid[cell.x, cell.y] = selectedTile;
            possibleTiles.Remove(cell);
        }
        else
        {
            Debug.LogError($"No possible tiles left for cell at {cell}");
        }
    }

    bool CheckCompatibility(Tile tile1, Tile tile2, string direction)
    {
        if (tile1 == null || tile2 == null)
        {
            Debug.LogError("Tile is null in IsCompatible check.");
            return false;
        }

        var key = (tile1, tile2, direction);
        if (!compatibilityCache.TryGetValue(key, out bool result))
        {
            result = tileManager.IsCompatible(tile1, tile2, direction);
            compatibilityCache[key] = result;
        }
        return result;
    }

    IEnumerator PropagateConstraints(Vector2Int cell)
    {
        Queue<Vector2Int> cellsToUpdate = new Queue<Vector2Int>();
        cellsToUpdate.Enqueue(cell);

        while (cellsToUpdate.Count > 0)
        {
            int processedCount = 0;

            while (cellsToUpdate.Count > 0 && processedCount < batchSize)
            {
                Vector2Int currentCell = cellsToUpdate.Dequeue();
                Tile currentTile = grid[currentCell.x, currentCell.y];

                if (currentTile == null)
                {
                    Debug.LogError($"Current tile is null at {currentCell}");
                    continue;
                }

                foreach (Vector2Int neighbor in GetNeighbors(currentCell))
                {
                    if (grid[neighbor.x, neighbor.y] == null)
                    {
                        List<Tile> neighborPossibleTiles = new List<Tile>(possibleTiles[neighbor]);

                        neighborPossibleTiles.RemoveAll(tile => !CheckCompatibility(currentTile, tile, GetDirection(currentCell, neighbor)));

                        if (neighborPossibleTiles.Count != possibleTiles[neighbor].Count)
                        {
                            possibleTiles[neighbor] = neighborPossibleTiles;
                            cellsToUpdate.Enqueue(neighbor);
                        }

                        if (neighborPossibleTiles.Count == 0)
                        {
                            Debug.LogError($"No possible tiles left for cell at {neighbor}");
                        }
                    }
                }

                processedCount++;
            }

            yield return null; // Wait until the next frame
        }
    }

    List<Vector2Int> GetNeighbors(Vector2Int cell)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        if (cell.x > 0) neighbors.Add(new Vector2Int(cell.x - 1, cell.y));
        if (cell.x < gridSizeX - 1) neighbors.Add(new Vector2Int(cell.x + 1, cell.y));
        if (cell.y > 0) neighbors.Add(new Vector2Int(cell.x, cell.y - 1));
        if (cell.y < gridSizeY - 1) neighbors.Add(new Vector2Int(cell.x, cell.y + 1));

        return neighbors;
    }

    string GetDirection(Vector2Int from, Vector2Int to)
    {
        if (to.x == from.x - 1) return "left";
        if (to.x == from.x + 1) return "right";
        if (to.y == from.y - 1) return "top";
        if (to.y == from.y + 1) return "bottom";
        return "";
    }

    void InstantiateTile(Vector2Int position, Tile tile)
    {
        if (tile.spriteIndex >= 0 && tile.spriteIndex < tileManager.tileSprites.Length)
        {
            Vector3 scaledPosition = new Vector3(position.x * 1.275f, 0, position.y * 1.275f);
            GameObject tileInstance = Instantiate(tilePrefab, scaledPosition, Quaternion.AngleAxis(90, Vector3.right));
            SpriteRenderer spriteRenderer = tileInstance.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = tileManager.tileSprites[tile.spriteIndex]; // Set the sprite based on the sprite index
        }
        else
        {
            Debug.LogError($"Sprite index {tile.spriteIndex} is out of bounds for tile at position {position}");
        }
    }

    void OnDrawGizmos()
    {
        if (grid == null) return;

        Gizmos.color = Color.red;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 position = new Vector3(x * 1.275f, 0, y * 1.275f);
                Gizmos.DrawWireCube(position, new Vector3(1.275f, 0, 1.275f));

                // Draw the sprite indices for each cell's possible tiles
                if (possibleTiles != null && possibleTiles.ContainsKey(new Vector2Int(x, y)))
                {
                    var possible = possibleTiles[new Vector2Int(x, y)];
                    string indices = string.Join(", ", possible.ConvertAll(tile => tile.spriteIndex.ToString()));
                    Handles.Label(position, indices);
                }
            }
        }
    }
}
