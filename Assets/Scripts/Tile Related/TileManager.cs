using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TileManager : MonoBehaviour
{
    // Script to spawn Tiles and handle Tile Dictionary
    // Need to find a way to save levels probably will use grid coordinates/dictionary and scriptable objects


    [Header("Tile Spawning")]
    public GameObject _tilePrefab; // Prefab for tiles
    [SerializeField] public int GridWidth = 10;
    [SerializeField] public int GridHeight = 10;
    [SerializeField] public float TileSize = 1f;

    [SerializeField] public float PercentageRiverTiles;
    [SerializeField] public float PercentageItemTiles;

    [SerializeField] public float NumberOfItems;

    [Header("River tiles Spawning")]
    public int minX;
    public int maxX;
    public int minY;
    public int maxY;

    [Header("Player Spawning")]
    public int playerX;
    public int playerY;

    [Header("Robot Spawning")]
    public int robotX;
    public int robotY;

    [Header("Breakable Tiles")]
    public int breakabletileX;
    public int breakabletileY;

    public Dictionary<Vector2Int, Tile> tileDictionary = new Dictionary<Vector2Int, Tile>(); // Take position and Tile type

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {

    }

    public void GenerateGrid()
    {
        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                Vector2 worldPosition = new Vector2(x * TileSize, y * TileSize); 

                GameObject tileObject = Instantiate(_tilePrefab, worldPosition, Quaternion.identity, transform);
                Tile tile = tileObject.GetComponent<Tile>();

                TileType type;

                if (position.x == robotX-1 && position.y == robotY-1) 
                {
                    type = TileType.TractorSpawn;
                    Debug.Log(position);
                }
                else if (position.x == playerX-1 && position.y == playerY-1) 
                {
                    type = TileType.PlayerSpawn;
                    Debug.Log(position);
                }
                else if (position.x >= minX && position.x <= maxX)
                {
                    if (position.y >= minY && position.y <= maxY)
                    {
                        type = TileType.River;
                    }
                    else
                    {
                        type = TileType.Normal;
                    }

                }

                else
                    {
                        type = TileType.Normal;
                    }

                    tile.Initialize(position, type); // Uses Initialize function in Tile.cs to spawn in the tiles
                    tileDictionary[position] = tile;  // and then this keeps track of what tile is where 
                
            }

            LogAllTilesInDictionary();
        }
    } 
    public bool IsTileAvailable(Vector2Int position) // Check if tile exists at a vector 2 position
    {
        return tileDictionary.ContainsKey(position);
    }
    public bool IsTileWalkable(Vector2Int position) // Walkable = Player
    {
        if (tileDictionary.ContainsKey(position))
        {
            return tileDictionary[position].IsWalkable;
        }
        return false;
    }

    public bool IsTileTraversable(Vector2Int position) // Traversable = Tractor
    {
        if (tileDictionary.ContainsKey(position))
        {
            return tileDictionary[position].IsTraversable;
        }
        return false;
    }

    public void LogAllTilesInDictionary() // Debug Log to check if tiles are functioning as intended for bugs
    {
        foreach (var tileKey in tileDictionary)
        {
            Vector2Int position = tileKey.Key;
            Tile tile = tileKey.Value;

            //Debug.Log($"Tile Position? : {position}, Tile Type? : {tile.tileType}, Walkable? : {tile.IsWalkable}, Traversable? : {tile.IsTraversable}");
        }
    }
}