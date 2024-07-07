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

public class TileManager : MonoBehaviour
{
    public Sprite[] tileSprites; // Array of sprites from the sprite sheet
    public Tile[] tiles; // Array of Tile objects
    public Dictionary<int, Dictionary<string, List<int>>> constraints;

    void Start()
    {
        // Load the sprites from the sprite sheet
        //tileSprites = Resources.LoadAll<Sprite>("Assets/WFCTilingTerrain/Tiles/MudGrass_TileSheet_001.png"); // Ensure the path matches your setup

        // Check if sprites are loaded correctly
        if (tileSprites == null || tileSprites.Length == 0)
        {
            Debug.LogError("Tile sprites not loaded. Check the path to the sprite sheet.");
            return;
        }

        // Initialize the tiles and constraints
        InitializeTiles();
        InitializeConstraints();
    }

    void InitializeTiles()
    {
        // tiles = new Tile[]
        // {
        //     new Tile(new string[] { "grass", "mud", "grass", "mud" }, 0),
        //     new Tile(new string[] { "mud", "grass", "mud", "grass" }, 1),
        //     new Tile(new string[] { "grass", "grass", "mud", "mud" }, 2),
        //     // Add other tiles with their edges and corresponding sprite index
        // };

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

        constraints[0] = new Dictionary<string, List<int>>()
        {
            { "top", new List<int>() { 0, 3, 12, 13 } },
            { "right", new List<int>() { 0, 4, 11, 12 } },
            { "bottom", new List<int>() { 0, 2, 10, 11 } },
            { "left", new List<int>() { 0, 5 , 10, 13 } }
        };

        constraints[1] = new Dictionary<string, List<int>>()
        {
            { "top", new List<int>() { 1 , 2 , 8 , 9} },
            { "right", new List<int>() { 1 , 5 , 7 , 8} },
            { "bottom", new List<int>() { 1 , 2 , 8 , 9} },
            { "left", new List<int>() { 1 , 4 , 6 , 9} }
        };

        constraints[2] = new Dictionary<string, List<int>>()
        {
            { "top", new List<int>() { 2 , 3 , 12, 13 } },
            { "right", new List<int>() { 2, 9 , 10} },
            { "bottom", new List<int>() { 1 , 6 , 7 } },
            { "left", new List<int>() { 2 , 8 , 11} }
        };
        
        constraints[3] = new Dictionary<string, List<int>>()
        {
            { "top", new List<int>() {  1 , 8 , 9} },
            { "right", new List<int>() { 3 , 6 , 13} },
            { "bottom", new List<int>() { 0 , 10, 11} },
            { "left", new List<int>() { 3 , 7 , 12} }
        };

        constraints[4] = new Dictionary<string, List<int>>()
        {
            { "top", new List<int>() { 4, 6, 11 } },
            { "right", new List<int>() { 1 , 5 , 7 , 8} },
            { "bottom", new List<int>() { 4 , 9 , 12} },
            { "left", new List<int>() { 0 , 5 , 10 , 13} }
        };

        constraints[5] = new Dictionary<string, List<int>>()
        {
            { "top", new List<int>() { 5 , 7 , 10} },
            { "right", new List<int>() { 0 , 4 , 11 , 12} },
            { "bottom", new List<int>() { 5 , 8 , 13} },
            { "left", new List<int>() { 1 , 4 , 6 , 8}  }
        };

        constraints[6] = new Dictionary<string, List<int>>()
        {
            { "top", new List<int>() { 1 , 2 , 8 , 9} },
            { "right", new List<int>() { 1 , 5 , 7 , 8} },
            { "bottom", new List<int>() { 4 , 9, 12} },
            { "left", new List<int>() { 3 , 7, 12} }
        };

        constraints[7] = new Dictionary<string, List<int>>()
        {
            { "top", new List<int>() { 1 , 2 , 8 , 9 } },
            { "right", new List<int>() { 3 , 6 , 13 } },
            { "bottom", new List<int>() { 5 , 8, 13} },
            { "left", new List<int>() { 1 , 4, 6 , 9} }
        };

        constraints[8] = new Dictionary<string, List<int>>()
        {
            { "top", new List<int>() { 5 , 7 , 10 } },
            { "right", new List<int>() { 2 , 9 , 10 } },
            { "bottom", new List<int>() { 1 , 3 , 6 , 7 } },
            { "left", new List<int>() { 1 , 4, 6, 9} }
        };

        constraints[9] = new Dictionary<string, List<int>>()
        {
            { "top", new List<int>() { 4 , 6 , 11 } },
            { "right", new List<int>() { 1 , 5, 7 , 8} },
            { "bottom", new List<int>() { 1 , 3 , 6 , 7 } },
            { "left", new List<int>() { 2 , 8, 11 } }
        };

        constraints[10] = new Dictionary<string, List<int>>()
        {
            { "top", new List<int>() { 0 , 3 , 12 , 13 } },
            { "right", new List<int>() { 0 , 4 , 11, 12} },
            { "bottom", new List<int>() { 5 , 7 , 13} },
            { "left", new List<int>() { 2 , 8 , 11 } }
        };

        constraints[11] = new Dictionary<string, List<int>>()
        {
            { "top", new List<int>() { 0 , 3 , 12 , 13 } },
            { "right", new List<int>() { 2, 9 , 10} },
            { "bottom", new List<int>() { 4 , 9 , 12} },
            { "left", new List<int>() { 0, 5 , 10, 13 } }
        };

        constraints[12] = new Dictionary<string, List<int>>()
        {
            { "top", new List<int>() { 4, 6, 11 } },
            { "right", new List<int>() { 3 , 6 , 13} },
            { "bottom", new List<int>() { 0, 2, 10, 11 } },
            { "left", new List<int>() { 0, 5 , 10, 13 } }
        };

        constraints[13] = new Dictionary<string, List<int>>()
        {
            { "top", new List<int>() { 5 , 7 , 10} },
            { "right", new List<int>() { 0, 4, 11, 12 } },
            { "bottom", new List<int>() { 0, 2, 10, 11 } },
            { "left", new List<int>() { 3 , 7 , 12} }
        };
    }

    public bool IsCompatible(Tile tile1, Tile tile2, string direction)
    {
        if (tile1 == null || tile2 == null)
        {
            Debug.LogError("Tile is null in IsCompatible check.");
            return false;
        }

        if (direction == "top")
            return tile1.edges[0] == tile2.edges[2] ;
        else if (direction == "right")
            return tile1.edges[1] == tile2.edges[3];
        else if (direction == "bottom")
            return tile1.edges[2] == tile2.edges[0];
        else if (direction == "left")
            return tile1.edges[3] == tile2.edges[1];
        return false;
    }
}