using UnityEngine;

public class TractorMovement : Movement, IWeightedObject
{
    [SerializeField] private TetherSystem tetherSystem;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float weight;
    [SerializeField] private float respawnCooldown = 2f; // Cooldown in seconds
    private float lastRespawnTime = -Mathf.Infinity; // Initialize to allow respawning immediately
    public Vector2Int CurrentDirection { get; private set; }


    protected override void Start()
    {
        base.Start();
        gameManager = FindObjectOfType<GameManager>();
        tetherSystem = GetComponent<TetherSystem>();
        SetTractorSpawnPosition();
    }

    private void SetTractorSpawnPosition()
    {
        foreach (var tileKey in tileManager.tileDictionary)
        {
            if (tileKey.Value.tileType == TileType.TractorSpawn)
            {
                currentPosition = tileKey.Key;
                Vector2 worldPosition = new Vector2(currentPosition.x * tileManager.TileSize, currentPosition.y * tileManager.TileSize);
                transform.position = worldPosition;
                Debug.Log($"Tractor spawned at: {worldPosition}");
            }
        }
    }

    public void CheckRespawn(Transform plateA, Transform plateB)
    {
        if (Time.time - lastRespawnTime < respawnCooldown)
        {
            Debug.Log($"{gameObject.name} respawn is on cooldown.");
            return;
        }

        Debug.Log($"CheckRespawn called for {gameObject.name} at position {transform.position}");

        if (transform.position == plateA.position)
        {
            Debug.Log($"{gameObject.name} is stepping on Plate A. Respawning the other character at Plate B.");
            gameManager.RespawnCharacter(gameObject, plateB.position);
        }
        else if (transform.position == plateB.position)
        {
            Debug.Log($"{gameObject.name} is stepping on Plate B. Respawning the other character at Plate A.");
            gameManager.RespawnCharacter(gameObject, plateA.position);
        }
        else
        {
            Debug.Log($"{gameObject.name} did not step on a valid respawn plate.");
        }

        lastRespawnTime = Time.time;
    }


    public void MoveToPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public float GetWeight()
    {
        return weight;
    }

    public void SetFacingDirection(Vector2Int direction)
    {
        CurrentDirection = direction;
        // Trigger any visual rotation/flip here
    }

}
