using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TetherSystem : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; // Reference to the player's transform
    [SerializeField] public int maxSteps = 6;          // Maximum distance (in steps) the player can move from the tractor
    private bool isOutsideTetherRange = false;         // Tracks whether the player is outside the range
    private Vector2Int previousPosition;
    private int previousMaxSteps;

    public bool IsCurrentlyActive = false; // Determines if the tether system is active
    private List<Tile> currentlyShieldedTiles = new List<Tile>();

    public List<Vector2Int> GetTilesWithinRange()
    {
        if (!IsCurrentlyActive)
        {
            return new List<Vector2Int>();
        }

        Vector2Int tractorPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        List<Vector2Int> tilesInRange = new List<Vector2Int>();

        for (int x = -maxSteps; x <= maxSteps; x++)
        {
            for (int y = -maxSteps; y <= maxSteps; y++)
            {
                Vector2Int offset = new Vector2Int(x, y);
                Vector2Int tilePosition = tractorPosition + offset;

                // Check if the tile is within the circular tether range
                if (Vector2Int.Distance(tractorPosition, tilePosition) <= maxSteps)
                {
                    tilesInRange.Add(tilePosition);
                }
            }
        }
        UpdateShieldedTiles(tilesInRange);
        //Debug.Log($"Number of tiles within range: {tilesInRange.Count}");
        return tilesInRange;
    }

    private void UpdateShieldedTiles(List<Vector2Int> tilesInRange)
    {
        // Get all active tiles from your tile manager or scene
        Tile[] allTiles = FindObjectsOfType<Tile>();

        foreach (Tile tile in currentlyShieldedTiles)
        {
            tile.SetIsShielded(false);
        }

        
        currentlyShieldedTiles = allTiles
            .Where(tile => tilesInRange.Contains(tile.GetGridPosition()))
            .ToList();

        foreach (Tile tile in currentlyShieldedTiles)
        {
            tile.SetIsShielded(true);
        }
    }

    void Start()
    {
        GameObject _playerObject = GameObject.FindWithTag("Player");
        if (_playerObject != null)
        {
            playerTransform = _playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player object not found in TetherSystem!");
        }

        if (IsCurrentlyActive)
        {
            TestTilesInRange();
        }
    }

    void Update()
    {
        

        if (!IsCurrentlyActive)
        {
            return;
        }

        Vector2Int currentPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

        // Check if position or maxSteps has changed

            GetTilesWithinRange(); // Recalculate tiles within range



        if (playerTransform == null)
        {
            return;
        }

        Vector2 playerPosition = playerTransform.position;
        //Vector2 currentPosition = transform.position;

        // Check the distance between the player and game object
        float distanceToCurrentGO = Vector2.Distance(playerPosition, currentPosition);

        if (distanceToCurrentGO > maxSteps)
        {
            if (!isOutsideTetherRange)
            {
                Debug.Log(name + ": You have moved outside the tether range!");
                isOutsideTetherRange = true;
            }
        }
        else
        {
            if (isOutsideTetherRange)
            {
                Debug.Log(name + ": You are back within the tether range.");
                isOutsideTetherRange = false;
            }
        }
    }

    void TestTilesInRange()
    {
        if (!IsCurrentlyActive)
        {
            return;
        }

        TetherSystem tetherSystem = GetComponent<TetherSystem>();
        List<Vector2Int> tilesInRange = GetTilesWithinRange();

        foreach (Vector2Int tile in tilesInRange)
        {
            //Debug.Log($"Tile in range: {tile}");
        }
    }

    public void CollectRopeItem()
    {
        if (!IsCurrentlyActive)
        {
            return;
        }

        maxSteps += 2;
        Debug.Log($"Tether range increased! Max Steps: {maxSteps}");
    }

    public int GetMaxSteps()
    {
        return maxSteps;
    }
}