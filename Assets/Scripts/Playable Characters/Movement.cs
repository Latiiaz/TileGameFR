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
        if (!isMoving && !isActionOnCooldown)
        {
            HandleInput();
            KnockedBackwards();
        }
        if (!tileManager.IsTileAvailable(currentPosition))
        {
            Debug.LogWarning($"Player is in an occupied tile at position {currentPosition}!");
        }
    }

    protected abstract void HandleInput(); // Implemented in derived classes

    public virtual void MoveOrTurn(Vector2Int direction)
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
        int tileLayerMask = LayerMask.GetMask("Tile"); // Only check for tiles
        int counter = 0;

        Collider2D hit = Physics2D.OverlapBox(transform.position, new Vector2(0.5f, 0.5f), 0, tileLayerMask);

        if (hit != null && hit.CompareTag("Door") && !isMoving) // Ensure player is not moving
        {
            Debug.Log($"Knocked back from a Door tile at {currentPosition}");

            Vector2Int oppositeDirection = -currentDirection;
            Vector2Int newPosition = currentPosition + oppositeDirection;

            // Keep moving backwards until a walkable tile is found
            while (!tileManager.IsTileAvailable(newPosition) || !tileManager.IsTileWalkable(newPosition))
            {
                counter++;
                if (counter >= 20)
                {
                    newPosition = currentPosition;
                }


                //Debug log this and find out where the crash happens, add restart function when the thing happens
                newPosition += oppositeDirection; // Move further back

            }

            // Move the player to the first available tile found
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

}
