using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorMovement : MonoBehaviour, IInteractable, IWeightedObject
{
    // Tractor Movement script referenced off playermovement script :D
    // Tether system to the player
    // Inability to move onto certain tiles such as the river

    [SerializeField] private TileManager _tileManager;
    private Vector2Int _tractorPosition = new Vector2Int(0, 0);
    private Vector2Int _tractorDirection = Vector2Int.left;
    private Vector2Int _tractorBackwards = new Vector2Int(0, -2);

    [SerializeField] private float _moveSpeed = 0.2f;
    [SerializeField] private float _actionCooldown = 0.2f;

    private bool _isMoving = false;
    private bool _isActionOnCooldown = false;

    [SerializeField] GameManager _gameManager;


    private PlayerMovement _player;

    private int _maxMoves; // Gain more steps based off cooldown

    [SerializeField] private float weight;
    private float _movementTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _tileManager = FindObjectOfType<TileManager>();
        _player = FindObjectOfType<PlayerMovement>();
        _gameManager = FindObjectOfType<GameManager>();


        SetTractorSpawnPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isMoving || _isActionOnCooldown)
        {

        }
        else
        {
            HandleInput();
        }
    }


    void SetTractorSpawnPosition()
    {
        foreach (var tileKey in _tileManager.tileDictionary)
        {
            if (tileKey.Value.tileType == TileType.TractorSpawn)
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
        if (Input.GetKey(KeyCode.LeftShift)) // When LeftShift is held, do not process player input
        {
            return;
        }

        // Player movement logic
        if (Input.GetKey(KeyCode.W))
            MoveForward(Vector2Int.up);
        else if (Input.GetKey(KeyCode.A))
            MoveForward(Vector2Int.left);
        else if (Input.GetKey(KeyCode.S))
            MoveForward(Vector2Int.down);
        else if (Input.GetKey(KeyCode.D))
            MoveForward(Vector2Int.right);
    }
    // Possible movement directions in clockwise order: UP, RIGHT, DOWN, LEFT
    private Vector2Int[] _directions = new Vector2Int[]
    {
    Vector2Int.up,    // (0,1)
    Vector2Int.right, // (1,0)
    Vector2Int.down,  // (0,-1)
    Vector2Int.left   // (-1,0)
    };

    private int _directionIndex = 0; // Tracks the current facing direction (0 = UP)

    // Rotates the tractor left (-1) or right (1) by adjusting the index in the _directions array
    void RotateTractor(int direction)
    {
        _directionIndex = (_directionIndex + direction + _directions.Length) % _directions.Length;
        _tractorDirection = _directions[_directionIndex];

        // Apply the new rotation (z-axis rotation since it's a 2D game)
        float newRotation = _directionIndex * -90f; // -90 for left-hand rotation order
        transform.rotation = Quaternion.Euler(0, 0, newRotation);
    }

    void MoveForward(Vector2Int direction)
    {
        if (_isMoving || _isActionOnCooldown)
        {
            return;
        }
        Vector2 tractorMovement = new Vector2(direction.x, direction.y);
        if (_tractorDirection != direction)
        {
            _tractorDirection = direction;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(direction.x, direction.y, 0));
            StartCoroutine(ActionCooldown());
        }
        else
        {
            Vector2Int newPosition = _tractorPosition + direction;

            if (_tileManager.IsTileAvailable(newPosition) && _tileManager.IsTileTraversable(newPosition))
            {
                StartCoroutine(MoveToPosition(newPosition));
            }
            else
            {
                Debug.LogWarning("(TRACTOR): Tile is not walkable or available.");
            }
        }
    }
    void MoveBackwards(Vector2Int direction)
    {
        if (_isMoving || _isActionOnCooldown)
        {
            return;
        }

        if (_tractorDirection != direction)
        {
            _tractorDirection = direction;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(direction.x, direction.y, 0));
            //_isActionOnCooldown = true;
        }
        else
        {
            Vector2Int newPosition = _tractorPosition - direction;

            if (_tileManager.IsTileAvailable(newPosition) && _tileManager.IsTileTraversable(newPosition))
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
        yield return new WaitForSeconds(_actionCooldown);
        _isActionOnCooldown = false;
    }

    IEnumerator MovementDelay()
    {
        _movementTimer = 0.1f; // Set delay time
        yield return new WaitForSeconds(_movementTimer);
    }
    //IEnumerator ActionCooldown()
    //{
    //    yield return new WaitForSeconds(_actionCooldown);
    //    _isActionOnCooldown = false;
    //}

    //public void InteractF() //Enter Exit tractor
    //{
    //    // TRACKTOR BRAINS!!!!!! (move the player inside)
    //    if (_player.IsInTractor)
    //    {

    //        _player.AttemptEnterOrExitTractor();
    //    }
    //    else
    //    {
    //        _player.AttemptEnterOrExitTractor();
    //    }
    //}

    public void InteractE()
    {
        //Debug.Log("E key hit interact");
    }

    //public Vector2Int GetTractorPosition()
    //{
    //    return _tractorPosition;
    //}

    public Vector2 GetFacingDirection()
    {
        // Example logic
        return transform.up; // Modify as needed based on your facing logic
    }
    public float GetWeight()
    {
        return weight;
    }
    IEnumerator ActionCooldown()
    {
        yield return new WaitForSeconds(_actionCooldown);
        _isActionOnCooldown = false;
    }
}
