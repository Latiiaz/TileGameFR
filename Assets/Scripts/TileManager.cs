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

    private Dictionary<Vector2Int, Tile> tileDictionary = new Dictionary<Vector2Int, Tile>();

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
                Vector2 worldPosition = new Vector2(x * TileSize, y * TileSize);

                GameObject tileObject = Instantiate(_tilePrefab, worldPosition, Quaternion.identity, transform);
                Tile tile = tileObject.GetComponent<Tile>();

                TileType type = (Random.value > 0.8f) ? TileType.River : TileType.Normal;
                tile.Initialize(position, type);
                tileDictionary[position] = tile;

            }
        }
    }

    public bool IsTileAvailable(Vector2Int position)
    {
        return tileDictionary.ContainsKey(position);
    }
    public bool IsTileWalkable(Vector2Int position)
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
            tile.SetWalkable(!tile.IsWalkable);
        }
    }

    public Tile GetTileAt(Vector2Int position)
    {
        if (tileDictionary.ContainsKey(position))
        {
            return tileDictionary[position];
        }
        return null;
    }

}