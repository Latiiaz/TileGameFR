using UnityEngine;

public class PlayerMovement : Movement, IWeightedObject
{
    [SerializeField] private TetherSystem tetherSystem;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float weight;

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

    public float GetWeight()
    {
        return weight;
    }
}
