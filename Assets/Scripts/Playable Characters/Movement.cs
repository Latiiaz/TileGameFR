using System.Collections;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    protected Vector2Int currentPosition;
    protected Vector2Int currentDirection = Vector2Int.up;

    [SerializeField] protected float moveSpeed = 0.2f;
    [SerializeField] protected float actionCooldown = 0.2f;

    protected bool isMoving = false;
    protected bool isActionOnCooldown = false;

    protected TileManager tileManager;

    protected virtual void Start()
    {
        tileManager = FindObjectOfType<TileManager>();
    }

    protected virtual void Update()
    {
        if (!isMoving && !isActionOnCooldown)
        {
            HandleInput();
        }
        if (!tileManager.IsTileAvailable(currentPosition))
        {
            Debug.LogWarning("Player is in an occupied tile!");
        }

    }

    protected abstract void HandleInput(); // Implemented in derived classes

    public virtual void MoveOrTurn(Vector2Int direction)
    {

        if (isMoving || isActionOnCooldown) return; // Prevents any input during cooldown

        if (currentDirection != direction)
        {
            currentDirection = direction;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(direction.x, direction.y, 0));
            StartCoroutine(ActionCooldown());
        }
        else
        {
            Vector2Int newPosition = currentPosition + direction;
            if (tileManager.IsTileAvailable(newPosition) && tileManager.IsTileWalkable(newPosition))
            {
                StartCoroutine(MoveToPosition(newPosition));
                StartCoroutine(ActionCooldown()); 
            }
        }
    }

    protected IEnumerator MoveToPosition(Vector2Int newPosition)
    {
        isMoving = true;
        isActionOnCooldown = true;

        Vector2 start = transform.position;
        Vector2 end = new Vector2(newPosition.x * tileManager.TileSize, newPosition.y * tileManager.TileSize);

        float elapsedTime = 0f;

        while (elapsedTime < moveSpeed)
        {
            transform.position = Vector2.Lerp(start, end, elapsedTime / moveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        currentPosition = newPosition;

        isMoving = false;
        isActionOnCooldown = false;
    }


    protected IEnumerator ActionCooldown()
    {
        if (isActionOnCooldown) yield break; // Prevent multiple cooldowns

        isActionOnCooldown = true;
        yield return new WaitForSeconds(actionCooldown);
        isActionOnCooldown = false;
    }

    public Vector2 GetFacingDirection()
    {
        return transform.up;
    }
}
