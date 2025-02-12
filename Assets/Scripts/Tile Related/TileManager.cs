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
        bool tractorSpawnPlaced = false;
        bool itemtestSpawnPlaced = false;

        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                Vector2 worldPosition = new Vector2(x * TileSize, y * TileSize); //Incase wanna increase tile size but probably kept at 1

                GameObject tileObject = Instantiate(_tilePrefab, worldPosition, Quaternion.identity, transform);
                Tile tile = tileObject.GetComponent<Tile>();

                TileType type;

                //if (position.x == 9 && position.y == 8) // For the tiles that only 1 of
                //{
                //    type = TileType.TractorSpawn;
                //    Debug.Log(position);
                //    //tractorSpawnPlaced = true;
                //}
                //else if (position.x == 1 && position.y == 1) // Spawn Objectivve Test Item
                //{
                //    type = TileType.PlayerSpawn;
                //    Debug.Log(position);
                //    //itemtestSpawnPlaced = true;
                //}
                //else // The more randomized ones and the normal tiles // These are for weird tiels 
                //{

                 if (position.x == robotX && position.y == robotY) // For the tiles that only 1 of
                {
                    type = TileType.TractorSpawn;
                    Debug.Log(position);
                    //tractorSpawnPlaced = true;
                }
                else if (position.x == playerX && position.y == playerY) // Spawn Objectivve Test Item
                {
                    type = TileType.PlayerSpawn;
                    Debug.Log(position);
                    //itemtestSpawnPlaced = true;
                }
                else if (position.x >= minX && position.x <= maxX) // Spawns 2 length river tile on the 6th and 7th X coordinate
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
                //else if (position.x == 9 && position.y == 8) // For the tiles that only 1 of
                //{
                //    type = TileType.TractorSpawn;
                //    Debug.Log(position);
                //    //tractorSpawnPlaced = true;
                //}
                //else if (position.x == 1 && position.y == 1) // Spawn Objectivve Test Item
                //{
                //    type = TileType.PlayerSpawn;
                //    Debug.Log(position);
                //    //itemtestSpawnPlaced = true;
                //}
                //else if (position.x == 13 || position.x == 12) // Spawns 2 length mud tile on the 11 and 12th X coordinate
                //{
                //    if (position.y == 0 || position.y == 1)
                //    {
                //        type = TileType.Normal;
                //    }
                //    else
                //    {
                //        type = TileType.Mud;
                //    }
                //}
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
        //public void ToggleTileWalkable(Vector2Int position)
        //{
        //    if (tileDictionary.ContainsKey(position))
        //    {
        //        Tile tile = tileDictionary[position];
        //        tile.SetWalkable(!tile.IsWalkable); // Changes the tile.cs script on the tile to toggle between walkable and not walkable
        //    }
        //}
    
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

            //Debug.Log($"Tile Position? : {position}, Tile Type? : {tile.tileType}, Walkable? : {tile.IsWalkable}, Traversable? : {tile.IsTraversable}");
        }
    }
}