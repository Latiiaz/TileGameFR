using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [Header("Tile Spawning")]
    public GameObject _tilePrefab;
    [SerializeField] public int GridWidth = 10;
    [SerializeField] public int GridHeight = 10;
    [SerializeField] public float TileSize = 1f;

    [Header("Player Spawning")]
    public int playerX;
    public int playerY;

    [Header("Robot Spawning")]
    public int robotX;
    public int robotY;

    public Dictionary<Vector2Int, Tile> tileDictionary = new Dictionary<Vector2Int, Tile>();

    public bool isGridGenerated { get; private set; } = false; // Flag to check if grid generation is complete
    public static TileManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // Ensure only one instance exists
    }


    public IEnumerator GenerateGridCoroutine()
    {
        isGridGenerated = false;

        List<Vector2Int> tilePositions = new List<Vector2Int>();

        // Generate a list of all possible tile positions
        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                tilePositions.Add(new Vector2Int(x, y));
            }
        }

        // Shuffle the tile positions randomly
        System.Random rng = new System.Random();
        int n = tilePositions.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (tilePositions[n], tilePositions[k]) = (tilePositions[k], tilePositions[n]);
        }

        // Spawn tiles in a random order
        foreach (Vector2Int position in tilePositions)
        {
            yield return new WaitForSeconds(0.015f); // Adds a delay for visual effect

            Vector2 worldPosition = new Vector2(position.x * TileSize, position.y * TileSize);
            GameObject tileObject = Instantiate(_tilePrefab, worldPosition, Quaternion.identity, transform);
            Tile tile = tileObject.GetComponent<Tile>();

            TileType type = TileType.Normal;
            if (position.x == robotX && position.y == robotY) type = TileType.TractorSpawn;
            else if (position.x == playerX && position.y == playerY) type = TileType.PlayerSpawn;

            tile.Initialize(position, type);
            tileDictionary[position] = tile;
        }

        isGridGenerated = true;
        yield break;
    }


    public bool IsTileAvailable(Vector2Int position)
    {
        return tileDictionary.ContainsKey(position);
    }
    public Tile GetTileAtPosition(Vector2Int position)
    {
        if (tileDictionary.TryGetValue(position, out Tile tile))
        {
            return tile;
        }
        return null; // Return null if no tile exists at this position
    }

}
