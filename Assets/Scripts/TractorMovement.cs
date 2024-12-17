using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorMovement : MonoBehaviour, IInteractable
{
    // Tractor Movement script referenced off playermovement script :D
    // Tether system to the player
    // Inability to move onto certain tiles such as the river

    [SerializeField] private TileManager _tileManager;
    private Vector2Int _tractorPosition = new Vector2Int(0, 0);
    private Vector2Int _tractorDirection = Vector2Int.up;

    [SerializeField] private float _moveSpeed = 0.2f;
    [SerializeField] private float _actionCooldown = 0.2f;

    private bool _isMoving = false;
    private bool _isActionOnCooldown = false;

    [SerializeField] GameManager _gameManager;


    private PlayerMovement _player;

    private int _maxMoves; // Gain more steps based off cooldown

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
        HandleInput();
        //if (_gameManager.TurnStatus())
        //{

        //}
        //else
        //{
        //    HandleInput();
        //}

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
        if (Input.GetKey(KeyCode.UpArrow))
            MoveOrTurn(Vector2Int.up);
        else if (Input.GetKey(KeyCode.LeftArrow))
            MoveOrTurn(Vector2Int.left);
        else if (Input.GetKey(KeyCode.DownArrow))
            MoveOrTurn(Vector2Int.down);
        else if (Input.GetKey(KeyCode.RightArrow))
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
            //_isActionOnCooldown = true;
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
}
