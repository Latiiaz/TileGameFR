using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorMovement : Movement, IWeightedObject
{
    [SerializeField] private TetherSystem tetherSystem;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float weight;

    protected override void Start()
    {
        base.Start();
        gameManager = FindObjectOfType<GameManager>();
        tetherSystem = GetComponent<TetherSystem>();
        SetTractorSpawnPosition();
    }

    protected override void HandleInput()
    {
        if (Input.GetKey(KeyCode.LeftShift)) return;

        if (Input.GetKey(KeyCode.W)) MoveOrTurn(Vector2Int.up);
        else if (Input.GetKey(KeyCode.A)) MoveOrTurn(Vector2Int.left);
        else if (Input.GetKey(KeyCode.S)) MoveOrTurn(Vector2Int.down);
        else if (Input.GetKey(KeyCode.D)) MoveOrTurn(Vector2Int.right);
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
    public override void MoveOrTurn(Vector2Int direction)
    {
        if (isMoving || isActionOnCooldown) return;

        if (currentDirection != direction)
        {
            currentDirection = direction;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(direction.x, direction.y, 0));
            StartCoroutine(ActionCooldown());
        }
        else
        {
            Vector2Int newPosition = currentPosition + direction;

            bool isTraversable = tileManager.IsTileTraversable(newPosition);
            bool isWalkable = tileManager.IsTileWalkable(newPosition);

            //Debug.Log($"(TRACTOR) Checking tile at {newPosition}: Walkable = {isWalkable}, Traversable = {isTraversable}");

            if (isTraversable)
            {
                StartCoroutine(MoveToPosition(newPosition));
                StartCoroutine(ActionCooldown());
            }
            else
            {
                //Debug.LogWarning("(TRACTOR): This Tile Cannot be moved to.");
            }
        }
    }


    public void CheckRespawn(Transform plateA, Transform plateB)
    {

        Debug.Log($"CheckRespawn called for {gameObject.name} at position {transform.position}");

        GameManager gameManager = FindObjectOfType<GameManager>();

        if (transform.position == plateA.position) // Shield stepped on Plate A
        {
            Debug.Log($"{gameObject.name} stepped on Plate A at {plateA.position}, respawning Player at {plateB.position}");
            gameManager.RespawnCharacter(gameManager._player, plateB.position); // Move Player
        }
        else if (transform.position == plateB.position) // Shield stepped on Plate B
        {
            Debug.Log($"{gameObject.name} stepped on Plate B at {plateB.position}, respawning Player at {plateA.position}");
            gameManager.RespawnCharacter(gameManager._player, plateA.position); // Move Player
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
