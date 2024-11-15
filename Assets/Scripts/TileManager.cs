using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    // Script to spawn Tiles and handle Tile Dictionary
    // Need to find a way to save levels probably will use grid coordinates/dictionary and scriptable objects


    [Header("Tile Spawning")]
    public GameObject _tilePrefab; // Prefab for tiles
    [SerializeField] public int GridWidth = 10;
    [SerializeField] public int GridHeight = 10;
    [SerializeField] public float TileSize = 1f;

    [SerializeField] public float PercentageRiverTiles = 0f;

    private Dictionary<Vector2Int, Tile> tileDictionary = new Dictionary<Vector2Int, Tile>(); // Take position and Tile type

    // Start is called before the first frame update
    void Start()
    {
    }

    public void GenerateGrid()
    {
        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                Vector2 worldPosition = new Vector2(x * TileSize, y * TileSize); //Incase wanna increase tile size but probably kept at 1

                GameObject tileObject = Instantiate(_tilePrefab, worldPosition, Quaternion.identity, transform);
                Tile tile = tileObject.GetComponent<Tile>();

                TileType type;
                if (Random.value < (PercentageRiverTiles/100))
                {
                    type = TileType.River;
                }
                else
                {
                    type = TileType.Normal;
                }
                tile.Initialize(position, type); // Uses Initialize function in Tile.cs to spawn in the tiles
                tileDictionary[position] = tile; // and then this keeps track of what tile is where 
               

            }
        }

        LogAllTilesInDictionary();
    }

    public bool IsTileAvailable(Vector2Int position) // Check if tile exists at a vector 2 position
    {
        return tileDictionary.ContainsKey(position);
    }
    public bool IsTileWalkable(Vector2Int position) // Check if tile is walkable at a vector 2 position
    {
        if (tileDictionary.ContainsKey(position))
        {
            return tileDictionary[position].IsWalkable;
        }
        return false;
    }
    public void ToggleTileWalkable(Vector2Int position)
    {
        if (tileDictionary.ContainsKey(position))
        {
            Tile tile = tileDictionary[position];
            tile.SetWalkable(!tile.IsWalkable); // Changes the tile.cs script on the tile to toggle between walkable and not walkable
        }
    }

    public Tile GetTileAt(Vector2Int position) // Not used yet but will be used to determine spawnpoint tile(s) later
    {
        if (tileDictionary.ContainsKey(position))
        {
            return tileDictionary[position];
        }
        return null;
    }

    public void LogAllTilesInDictionary() // Debug Log to check if tiles are functioning as intended for bugs
    {
        foreach (var tileKey in tileDictionary)
        {
            Vector2Int position = tileKey.Key;
            Tile tile = tileKey.Value;

            Debug.Log($"Tile Position? : {position}, Tile Type? : {tile.tileType}, Walkable? : {tile.IsWalkable}");
        }
    }
}