using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    // Script to Handle Movement of player on tiles/grids
    // Player needs to communicate with dictionary to ensure they dont teleport back to starting point before getting into the tractor

    private TileManager _tileManager;
    private Vector2Int _playerPosition;
    private Vector2Int _playerDirection = Vector2Int.up;

    [SerializeField] private float _moveSpeed = 0.2f;
    [SerializeField] private float _actionCooldown = 0.2f;

    private bool _isMoving = false;
    private bool _isActionOnCooldown = false;

    private TractorMovement _tractorMovement;
    private ItemSystem _itemSystem;
    public bool IsInTractor { get; set; } = false; // If true the player starts off as the tractor lol.

    public bool IsCarryingItem { get;set; } = false; // Carrying items

    private TetherSystem _tetherSystem;
    //public GameManager _gameManager;
    //public SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        _tileManager = FindObjectOfType<TileManager>();
        _tractorMovement = FindObjectOfType<TractorMovement>();
        _itemSystem = FindObjectOfType<ItemSystem>();

        _tetherSystem = GetComponent<TetherSystem>();

        //transform.position = new Vector2(_playerPosition.x * _tileManager.TileSize, _playerPosition.y * _tileManager.TileSize);
        SetPlayerSpawnPosition();
       
        _tractorMovement.InteractF();

    }

    // Update is called once per frame
    void Update()
    {
        if (IsInTractor)
        {
            return;
        }
        if (IsCarryingItem)
        {
            return;
        }
           
        if (!_isActionOnCooldown && _tetherSystem != null) // !IsInTractor can hide to pair with movement of tractor // Tethersystem can move removed to test
        {
            HandleInput();
        }
    }

    void SetPlayerSpawnPosition() // Sets the player spawn location
    {
        foreach (var tileKey in _tileManager.tileDictionary)
        {
            if (tileKey.Value.tileType == TileType.TractorSpawn)
            {
                _playerPosition = tileKey.Key; 
                Vector2 worldPosition = new Vector2(_playerPosition.x * _tileManager.TileSize, _playerPosition.y * _tileManager.TileSize);
                transform.position = worldPosition;
                Debug.Log($"Player spawned at: {worldPosition}");
            }
        }
    }


    void HandleInput() // Handles the players inputs 
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

    void MoveOrTurn(Vector2Int direction) // Turns the player or moves them based off HandleInput
    {
        if (_isMoving || _isActionOnCooldown)
        {
            return;
        }

        Vector2 playerMovement = new Vector2(direction.x, direction.y);

        if (_tetherSystem.CanMoveTowardTractor(playerMovement)) // Tether only allows movement if the steps is going to be less than max steps to prevent moving out of the amx steps range
        {
            if (_playerDirection != direction)
            {
                _playerDirection = direction;
                transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(direction.x, direction.y, 0));
                StartCoroutine(ActionCooldown());
            }
            else
            {
                Vector2Int newPosition = _playerPosition + direction;

                if (_tileManager.IsTileAvailable(newPosition) && _tileManager.IsTileWalkable(newPosition)) // This is what handles the dictionary for the player & tile walkasbility
                {
                    StartCoroutine(MoveToPosition(newPosition));
                }
                else
                {
                    Debug.LogWarning("(PLAYER): Tile is not walkable or available.");
                }
            }
        }
        else
        {
            Debug.Log("Player is too far away from the tractor to move");
        }
    }

    public void AttemptEnterOrExitTractor() // Tractor Enter Exit
    {
        if (_tractorMovement != null && !IsInTractor)
        {
            if (_tractorMovement.transform.position == transform.position)
            {
                EnterTractor();
            }
        }
        else if (IsInTractor)
        {
            ExitTractor();
        }
    }

    public void EnterTractor()
    {
        IsInTractor = true;
        _isMoving = false;
        _isActionOnCooldown = true;

        transform.SetParent(_tractorMovement.transform);
        transform.localPosition = Vector3.zero;
        transform.rotation = _tractorMovement.transform.rotation;

        Debug.Log("Player has entered the tractor.");
        StartCoroutine(ActionCooldown());
    }

    public void ExitTractor()
    {
        IsInTractor = false;
        _isActionOnCooldown = true;

        transform.SetParent(null);
        Vector2Int targetPosition = _tractorMovement.GetTractorPosition();

        if (_tileManager.IsTileAvailable(targetPosition))
        {
            _playerPosition = targetPosition;
            transform.position = new Vector2(targetPosition.x * _tileManager.TileSize, targetPosition.y * _tileManager.TileSize);
        }
        else
        {
            Debug.LogWarning("Exiting tractor to an unavailable tile.");
        }

        Debug.Log("Player has exited the tractor.");
        StartCoroutine(ActionCooldown());
    }

    public void AttemptCarryOrDropItem() // Item drop/ pick up
    {
        if (_itemSystem != null && !IsCarryingItem)
        {
            if (_itemSystem.transform.position == transform.position)
            {
                CarryItem();
            }
        }
        else if (IsCarryingItem)
        {
            DropItem();
        }
    }

    public void CarryItem()
    {
        IsCarryingItem = true;
        _isMoving = false;
        _isActionOnCooldown = true;

        transform.SetParent(_itemSystem.transform);
        transform.localPosition = Vector3.zero;
        transform.rotation = _itemSystem.transform.rotation;

        Debug.Log("Player has picked up item.");
        StartCoroutine(ActionCooldown());
    }
    public void DropItem()
    {
        IsCarryingItem = false;
        _isActionOnCooldown = true;

        transform.SetParent(null);
        Vector2Int targetPosition = _itemSystem.GetTractorPosition();

        if (_tileManager.IsTileAvailable(targetPosition))
        {
            _playerPosition = targetPosition;
            transform.position = new Vector2(targetPosition.x * _tileManager.TileSize, targetPosition.y * _tileManager.TileSize);
        }
        else
        {
            Debug.LogWarning("Exiting item to an unavailable tile.");
        }

        Debug.Log("Player has dropped the item.");
        StartCoroutine(ActionCooldown());
    }

    void CheckCurrentTile() //Wee woo
    {
        if (_tileManager.IsTileAvailable(_playerPosition))
        {
            Debug.Log("Player is now on tile: " + _playerPosition);
        }
        else
        {
            Debug.LogWarning("No tile at player position: " + _playerPosition);
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
        _playerPosition = newPosition;

        _isMoving = false;
        StartCoroutine(ActionCooldown());

        CheckCurrentTile();
    }

    IEnumerator ActionCooldown()
    {
        yield return new WaitForSeconds(_actionCooldown);
        _isActionOnCooldown = false;
    }
}