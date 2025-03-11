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
    public float GetWeight()
    {
        return weight;
    }
}
