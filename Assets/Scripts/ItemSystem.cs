using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSystem : MonoBehaviour, IInteractable
{
    //Same as tractor script to handle "carrying" 
    // Tether system to the player
    // Inability to move onto certain tiles such as the river

    [SerializeField] private TileManager _tileManager;
    private Vector2Int _tractorPosition = new Vector2Int(0, 0);
    private Vector2Int _tractorDirection = Vector2Int.up;

    [SerializeField] private float _moveSpeed = 0.2f;
    [SerializeField] private float _actionCooldown = 0.2f;

    private bool _isMoving = false;
    private bool _isActionOnCooldown = false;

    private PlayerMovement _player;
    private ObjectiveSystem objectiveSystem;

    // Start is called before the first frame update
    void Start()
    {
        _tileManager = FindObjectOfType<TileManager>();
        _player = FindObjectOfType<PlayerMovement>();
        objectiveSystem = FindObjectOfType<ObjectiveSystem>();

        SetTractorSpawnPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.IsCarryingItem)
        {
            HandleInput();
        }
    }


    void SetTractorSpawnPosition() // Item spawn position
    {
        foreach (var tileKey in _tileManager.tileDictionary)
        {
            if (tileKey.Value.tileType == TileType.ObjectiveSpawn)
            {
                _tractorPosition = tileKey.Key;
                Vector2 worldPosition = new Vector2(_tractorPosition.x * _tileManager.TileSize, _tractorPosition.y * _tileManager.TileSize);
                transform.position = worldPosition;
                Debug.Log($"Tractor spawned at: {worldPosition}");
            }
        }
    }


    void HandleInput()
    {
        if (Input.GetKey(KeyCode.W))
            MoveOrTurn(Vector2Int.up);
        else if (Input.GetKey(KeyCode.A))
            MoveOrTurn(Vector2Int.left);
        else if (Input.GetKey(KeyCode.S))
            MoveOrTurn(Vector2Int.down);
        else if (Input.GetKey(KeyCode.D))
            MoveOrTurn(Vector2Int.right);
    }
    void MoveOrTurn(Vector2Int direction)
    {
        if (_isMoving || _isActionOnCooldown)
        {
            return;
        }

        if (_tractorDirection != direction)
        {
            _tractorDirection = direction;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(direction.x, direction.y, 0));
            StartCoroutine(ActionCooldown());
        }
        else
        {
            Vector2Int newPosition = _tractorPosition + direction;

            if (_tileManager.IsTileAvailable(newPosition) && _tileManager.IsTileWalkable(newPosition))
            {
                StartCoroutine(MoveToPosition(newPosition));
            }
            else
            {
                Debug.LogWarning("(TRACTOR): Tile is not walkable or available.");
            }
        }
    }

    IEnumerator MoveToPosition(Vector2Int newPosition)
    {
        _isMoving = true;
        _isActionOnCooldown = true;

        Vector2 start = transform.position;
        Vector2 end = new Vector2(newPosition.x * _tileManager.TileSize, newPosition.y * _tileManager.TileSize);

        float elapsedTime = 0f;

        while (elapsedTime < _moveSpeed)
        {
            transform.position = Vector2.Lerp(start, end, elapsedTime / _moveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        _tractorPosition = newPosition;

        _isMoving = false;
        StartCoroutine(ActionCooldown());
    }

    IEnumerator ActionCooldown()
    {
        yield return new WaitForSeconds(_actionCooldown);
        _isActionOnCooldown = false;
    }

    public void InteractE() //Enter Exit tractor
    {
        // TRACKTOR BRAINS!!!!!! (move the player inside)
        if (_player.IsCarryingItem)
        {
            _player.AttemptCarryOrDropItem();
        }
        else
        {
            _player.AttemptCarryOrDropItem();
        }
    }
    public Vector2Int GetTractorPosition()
    {
        return _tractorPosition;
    }

    void OnCollisionStay2D(Collision2D collision) // interacting with the cart
    {
        if (collision.gameObject.CompareTag("Cart"))
        {
            _player.AttemptCarryOrDropItem();
            Destroy(this.gameObject);
            objectiveSystem._objectiveCount++;
            Debug.Log("Item Deposited, Victory Sequence");
        }
    }
}
