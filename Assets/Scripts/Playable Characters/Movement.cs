using System;
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

    protected Transform objectTransform;

    protected TileManager tileManager;
    private BoxCollider2D boxCollider;

    [SerializeField] protected float KBmoveSpeed = 0f;


    [SerializeField] protected AudioClip moveSound;
    protected AudioSource audioSource;
    [SerializeField] private Transform spriteTransform;


    protected virtual void Start()
    {
        tileManager = FindObjectOfType<TileManager>();
        boxCollider = GetComponent<BoxCollider2D>();
        objectTransform = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();

        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider2D missing on Movement object!");
        }
        else
        {
            boxCollider.isTrigger = true;
        }
    }

    protected virtual void Update()
    {
        KnockedBackwards();

        if (!tileManager.IsTileAvailable(currentPosition))
        {
            Debug.LogWarning($"Player is in an occupied tile at position {currentPosition}!");
        }
    }

    public virtual void MoveOrTurn(Vector2Int direction)
    {
        if (isMoving || isActionOnCooldown) return;

        RotateToDirection(direction); // Still here, in case others use this version

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
        currentDirection = direction;

        if (spriteTransform == null) return;

        if (direction == Vector2Int.up)
        {
            spriteTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction == Vector2Int.down)
        {
            spriteTransform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (direction == Vector2Int.left)
        {
            spriteTransform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (direction == Vector2Int.right)
        {
            spriteTransform.rotation = Quaternion.Euler(0, 0, -90);
        }
    }





    // NEW: No-rotation version used by GameManager
    public void MoveInDirection(Vector2Int direction)
    {
        if (isMoving || isActionOnCooldown) return;

        Vector2Int newPosition = currentPosition + direction;
        if (tileManager.IsTileAvailable(newPosition))
        {
            StartCoroutine(MoveToPosition(newPosition));
            StartCoroutine(ActionCooldown());
        }
    }

    protected IEnumerator MoveToPosition(Vector2Int newPosition)
    {
        isMoving = true;
        isActionOnCooldown = true;
        if (audioSource != null && moveSound != null)
        {
            audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
            audioSource.PlayOneShot(moveSound);
        }
        Vector2 start = transform.position;
        Vector2 end = new Vector2(newPosition.x * tileManager.TileSize, newPosition.y * tileManager.TileSize);

        Vector2Int direction = newPosition - currentPosition;

        // Apply squash/stretch based on direction
        if (Mathf.Abs(direction.y) > 0)
        {
            transform.localScale = new Vector3(0.8f, 1.2f, 1);
        }
        else if (Mathf.Abs(direction.x) > 0)
        {
            transform.localScale = new Vector3(1.2f, 0.8f, 1);
        }

        // Play move sound
        if (audioSource != null && moveSound != null)
        {
            audioSource.PlayOneShot(moveSound);
        }

        float elapsedTime = 0f;
        while (elapsedTime < moveSpeed)
        {
            transform.position = Vector2.Lerp(start, end, elapsedTime / moveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        currentPosition = newPosition;
        transform.localScale = Vector3.one;

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
        if (isMoving) return;

        int tileLayerMask = LayerMask.GetMask("Tile");
        Collider2D hit = Physics2D.OverlapBox(transform.position, new Vector2(0.5f, 0.5f), 0, tileLayerMask);

        if (hit != null && hit.CompareTag("Door"))
        {
            Debug.Log($"Knocked back from a Door tile at {currentPosition}");

            Vector2Int oppositeDirection = -currentDirection;
            Vector2Int newPosition = currentPosition + oppositeDirection;

            while (!tileManager.IsTileAvailable(newPosition))
            {
                newPosition += oppositeDirection;
            }

            Debug.Log($"Knocking back to {newPosition}");

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
