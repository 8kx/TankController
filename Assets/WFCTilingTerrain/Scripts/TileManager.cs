using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile
{
    public string[] edges; // Edges in the order: [top, right, bottom, left]
    public int spriteIndex; // Index of the sprite in the sprite sheet

    public Tile(string[] edges, int spriteIndex)
    {
        this.edges = edges;
        this.spriteIndex = spriteIndex;
    }
}

[System.Serializable]
public class Edge
{
    public string type; // e.g., "mud", "grass"
    public string compatibility; // e.g., "A", "B"

    public Edge(string type, string compatibility)
    {
        this.type = type;
        this.compatibility = compatibility;
    }
}

public class TileManager : MonoBehaviour
{
    public Sprite[] tileSprites; // Array of sprites from the sprite sheet
    public Tile[] tiles; // Array of Tile objects
    public Dictionary<int, Dictionary<string, List<int>>> constraints;

    void Start()
    {
        // Load the sprites from the sprite sheet
        // Ensure the path matches your setup
        // tileSprites = Resources.LoadAll<Sprite>("Assets/WFCTilingTerrain/Tiles/MudGrass_TileSheet_001.png");

        // Check if sprites are loaded correctly
        if (tileSprites == null || tileSprites.Length == 0)
        {
            Debug.LogError("Tile sprites not loaded. Check the path to the sprite sheet.");
            return;
        }

        // Initialize the tiles and constraints
        //InitializeTiles();
        InitializeConstraints();
    }

    void InitializeTiles()
    {
        // Example initialization of tiles
        tiles = new Tile[]
        {
            new Tile(new string[] { "mud,mud", "mud,mud", "mud,mud", "mud,mud" }, 0),
            new Tile(new string[] { "grass,grass", "grass,grass", "grass,grass", "grass,grass" }, 1),
            // Add more tiles as needed
        };

        // Check if tile definitions are correct
        foreach (var tile in tiles)
        {
            if (tile.spriteIndex < 0 || tile.spriteIndex >= tileSprites.Length)
            {
                Debug.LogError($"Tile with edges {string.Join(", ", tile.edges)} has an invalid sprite index {tile.spriteIndex}");
            }
        }
    }

    void InitializeConstraints()
    {
        constraints = new Dictionary<int, Dictionary<string, List<int>>>();

        // Add your constraint initialization logic here
        for (int i = 0; i < tiles.Length; i++)
        {
            constraints[i] = new Dictionary<string, List<int>>();
            constraints[i]["top"] = new List<int>(); // Fill with compatible tiles' indices
            constraints[i]["right"] = new List<int>();
            constraints[i]["bottom"] = new List<int>();
            constraints[i]["left"] = new List<int>();
        }
    }

    public bool IsCompatible(Tile tile1, Tile tile2, string direction)
    {
        if (tile1 == null || tile2 == null)
        {
            Debug.LogError("Tile is null in IsCompatible check.");
            return false;
        }

        switch (direction)
        {
            case "top":
                return tile1.edges[0] == tile2.edges[2];
            case "right":
                return tile1.edges[1] == tile2.edges[3];
            case "bottom":
                return tile1.edges[2] == tile2.edges[0];
            case "left":
                return tile1.edges[3] == tile2.edges[1];
            default:
                return false;
        }
    }
}
