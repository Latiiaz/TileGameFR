using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Movement : MonoBehaviour
{
    public Vector2Int currentPosition;
    protected Vector2Int currentDirection = Vector2Int.up;

    [SerializeField] protected float moveSpeed = 0.2f;
    [SerializeField] protected float actionCooldown = 0.2f;
    [SerializeField] public bool isMoving = false;
    public bool isActionOnCooldown = false;

    protected TileManager tileManager;
    private BoxCollider2D boxCollider;

    [SerializeField] protected float KBmoveSpeed = 0f;

    protected virtual void Start()
    {
        tileManager = FindObjectOfType<TileManager>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider2D missing on Movement object!");
        }
        else
        {
            boxCollider.isTrigger = true; // Ensure it works with triggers
        }
    }

    protected virtual void Update()
    {
        KnockedBackwards(); // Always check for knockback, even while moving.

        if (!tileManager.IsTileAvailable(currentPosition))
        {
            Debug.LogWarning($"Player is in an occupied tile at position {currentPosition}!");
        }
    }


    public virtual void MoveOrTurn(Vector2Int direction)
    {
        if (isMoving || isActionOnCooldown) return;

        // Always rotate first
        RotateToDirection(direction);

        // If already facing the direction, attempt movement
        if (currentDirection == direction)
        {
            Vector2Int newPosition = currentPosition + direction;
            if (tileManager.IsTileAvailable(newPosition))
            {
                StartCoroutine(MoveToPosition(newPosition));
                StartCoroutine(ActionCooldown());
            }
        }
    }

    public void RotateToDirection(Vector2Int direction)
    {
        if (currentDirection != direction) // Only rotate if facing a new direction
        {
            currentDirection = direction;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
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
        if (isActionOnCooldown) yield break;

        isActionOnCooldown = true;
        yield return new WaitForSeconds(actionCooldown);
        isActionOnCooldown = false;
    }

    public Vector2 GetFacingDirection()
    {
        return transform.up;
    }
    private void KnockedBackwards()
    {
        if (isMoving) return; // Prevent knockback while moving normally.

        int tileLayerMask = LayerMask.GetMask("Tile");

        Collider2D hit = Physics2D.OverlapBox(transform.position, new Vector2(0.5f, 0.5f), 0, tileLayerMask);

        if (hit != null && hit.CompareTag("Door"))
        {
            Debug.Log($"Knocked back from a Door tile at {currentPosition}");

            Vector2Int oppositeDirection = -currentDirection;
            Vector2Int newPosition = currentPosition + oppositeDirection;

            // Keep moving back **until a walkable tile is found**
            while (!tileManager.IsTileAvailable(newPosition))
            {
                newPosition += oppositeDirection; // Keep moving in the same direction
            }

            Debug.Log($"Knocking back to {newPosition}");

            // Stop any movement and start knockback
            StopCoroutine("MoveToPosition");
            StopCoroutine("KnockBackMovement");

            StartCoroutine(KnockBackMovement(newPosition));
        }
    }


    protected IEnumerator KnockBackMovement(Vector2Int newPosition)
    {
        isMoving = true;
        isActionOnCooldown = true;

        Vector2 start = transform.position;
        Vector2 end = new Vector2(newPosition.x * tileManager.TileSize, newPosition.y * tileManager.TileSize);

        float elapsedTime = 0f;

        while (elapsedTime < KBmoveSpeed)
        {
            transform.position = Vector2.Lerp(start, end, elapsedTime / KBmoveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        currentPosition = newPosition;

        isMoving = false;
        isActionOnCooldown = false;
    }

    public Vector2Int GetGridPosition()
    {
        return Vector2Int.RoundToInt(transform.position); 
    }

}
