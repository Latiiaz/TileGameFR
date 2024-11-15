using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    // Script to Handle Movement of player on tiles/grids
    // Player needs to communicate with dictionary to ensure they dont teleport back to starting point before getting into the tractor

    [SerializeField] private TileManager _tileManager;
    private Vector2Int _playerPosition = new Vector2Int(0, 0);
    private Vector2Int _playerDirection = Vector2Int.up;

    [SerializeField] private float _moveSpeed = 0.2f;
    [SerializeField] private float _actionCooldown = 0.2f;

    private bool _isMoving = false;
    private bool _isActionOnCooldown = false;

    private TractorMovement _tractorMovement;
    public bool IsInTractor { get; private set; } = false; // If true the player starts off as the tractor lol.

    //public GameManager _gameManager;
    //public SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        _tileManager = FindObjectOfType<TileManager>();
        _tractorMovement = FindObjectOfType<TractorMovement>();
        // spriteRenderer = FindObjectOfType<SpriteRenderer>(); Hiding the player object not needed
        //_playerPosition = _gameManager._playerStartPosition; 
        //Debug.Log("(PLAYER): " + transform.position);
        transform.position = new Vector2(_playerPosition.x * _tileManager.TileSize, _playerPosition.y * _tileManager.TileSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isActionOnCooldown) // !IsInTractor can hide to pair with movement of tractor
        {
            HandleInput();
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

        if (_playerDirection != direction)
        {
            _playerDirection = direction;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(direction.x, direction.y, 0));
            StartCoroutine(ActionCooldown());
        }
        else
        {
            Vector2Int newPosition = _playerPosition + direction;
           

            if (_tileManager.IsTileAvailable(newPosition) && _tileManager.IsTileWalkable(newPosition)) //This is what handles the dictionary for the player
            {
                StartCoroutine(MoveToPosition(newPosition));
            }
            else
            {
                Debug.LogWarning("(PLAYER): Tile is not walkable or available.");
            }
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
        Vector2Int currentTile = _playerPosition;
        transform.position = _tractorMovement.transform.position;
        transform.rotation = _tractorMovement.transform.rotation;

        Debug.Log("Player has exited the tractor.");
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