using UnityEngine;

public class PlayerMovement : Movement, IWeightedObject
{
    [SerializeField] private TetherSystem tetherSystem;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float weight;
    [SerializeField] private float respawnCooldown = 2f; // Cooldown in seconds
    private float lastRespawnTime = -Mathf.Infinity; // Initialize to allow respawning immediately

    protected override void Start()
    {
        base.Start();
        gameManager = FindObjectOfType<GameManager>();
        tetherSystem = GetComponent<TetherSystem>();
        SetPlayerSpawnPosition();
    }

    protected override void HandleInput()
    {
        if (Input.GetKey(KeyCode.LeftShift)) return;

        if (Input.GetKey(KeyCode.W)) MoveOrTurn(Vector2Int.up);
        else if (Input.GetKey(KeyCode.A)) MoveOrTurn(Vector2Int.left);
        else if (Input.GetKey(KeyCode.S)) MoveOrTurn(Vector2Int.down);
        else if (Input.GetKey(KeyCode.D)) MoveOrTurn(Vector2Int.right);
    }

    private void SetPlayerSpawnPosition()
    {
        foreach (var tileKey in tileManager.tileDictionary)
        {
            if (tileKey.Value.tileType == TileType.PlayerSpawn)
            {
                currentPosition = tileKey.Key;
                Vector2 worldPosition = new Vector2(currentPosition.x * tileManager.TileSize, currentPosition.y * tileManager.TileSize);
                transform.position = worldPosition;
                Debug.Log($"Player spawned at: {worldPosition}");
            }
        }
    }

    public void CheckRespawn(Transform plateA, Transform plateB)
    {
        // Check if the cooldown period has passed
        if (Time.time - lastRespawnTime < respawnCooldown)
        {
            Debug.Log($"{gameObject.name} respawn is on cooldown ({Time.time - lastRespawnTime}/{respawnCooldown} seconds passed).");
            return;
        }

        Debug.Log($"CheckRespawn called for {gameObject.name} at position {transform.position}");

        if (transform.position == plateA.position) // Player stepped on Plate A
        {
            Debug.Log($"{gameObject.name} stepped on Plate A at {plateA.position}, respawning Tractor at {plateB.position}");
            gameManager.RespawnCharacter(gameManager._tractor, plateB.position); // Move Player
            lastRespawnTime = Time.time; // Update last respawn time
        }
        else if (transform.position == plateB.position) // Player stepped on Plate B
        {
            Debug.Log($"{gameObject.name} stepped on Plate B at {plateB.position}, respawning Tractor at {plateA.position}");
            gameManager.RespawnCharacter(gameManager._tractor, plateA.position); // Move Player
            lastRespawnTime = Time.time; // Update last respawn time
        }
        else
        {
            Debug.Log($"{gameObject.name} did not step on a valid respawn plate.");
        }
    }

    public void MoveToPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public float GetWeight()
    {
        return weight;
    }
}
