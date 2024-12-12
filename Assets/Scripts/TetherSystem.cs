using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherSystem : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; // Reference to the player's transform
    [SerializeField] public int maxSteps = 6;          // Maximum distance (in steps) the player can move from the tractor
    private bool isOutsideTetherRange = false;         // Tracks whether the player is outside the range

    public bool IsCurrentlyActive = false; // Determines if the tether system is active

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

        Debug.Log($"Number of tiles within range: {tilesInRange.Count}");
        return tilesInRange;
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




        if (playerTransform == null)
        {
            return;
        }

        Vector2 playerPosition = playerTransform.position;
        Vector2 currentPosition = transform.position;

        // Check the distance between the player and the tractor
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
        List<Vector2Int> tilesInRange = tetherSystem.GetTilesWithinRange();

        foreach (Vector2Int tile in tilesInRange)
        {
            Debug.Log($"Tile in range: {tile}");
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