using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    // Script to Handle Spawning of player and tilebox checks
    // Need to add dont destroy on load like level manager

    public GameObject PlayerPrefab;
    public GameObject TractorPrefab;
    public GameObject CartPrefab;
    public GameObject ItemTestPrefab;
    public GameObject PylonPrefab;

    private GameObject _player;
    private GameObject _tractor;
    private GameObject _cart;
    private GameObject _itemTest;
    private GameObject _pylon;

    //They spawn on dedicated tiles for them now but player should spawn INSIDE of the tractor and the cart should always be behind the tractor
    private Vector2Int _playerStartPosition = new Vector2Int(0, 0);
    private Vector2Int _tractorStartPosition = new Vector2Int(0, 0);
    private Vector2Int _cartStartPosition = new Vector2Int(0, 0);
    private Vector2Int _itemTestStartPosition = new Vector2Int(0, 0);
    private Vector2Int _pylonStartPosition = new Vector2Int(0, 0);

    public LevelManager levelManager;
    public TileManager tileManager;

    [Header("Main Character")]
    public bool _currentTurnIsPlayer = true;
    // Start is called before the first frame update
    void Start() // Spawns the map player tractor and cart
    {
        tileManager.GenerateGrid(); // Spawns grids so all tiles have spawned
        
        SpawnTractor();
      
        SpawnPlayer();
    }
    private void Awake()
    {
        if (_player == null)
        {
            _player = GameObject.FindWithTag("Player");
        }
        if (_tractor == null)
        {
            _tractor = GameObject.FindWithTag("Tractor");
        }
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F)) No need for turn changing
        //{

        //    SwitchTurn();
        //} 
    }

    public void SwitchTurn()
    {
        _currentTurnIsPlayer = !_currentTurnIsPlayer;
        TurnChecker();
    }

    public void TurnChecker()
    {
    }

    public bool TurnStatus()
    {
        return _currentTurnIsPlayer;
    }


    void SpawnPlayer()
    {
        if (tileManager.IsTileAvailable(_playerStartPosition))
        {
            Vector2 spawnPosition = new Vector2(_playerStartPosition.x * tileManager.TileSize, _playerStartPosition.y * tileManager.TileSize);
            _player = Instantiate(PlayerPrefab, spawnPosition, Quaternion.identity);
            //Debug.Log("(PLAYER): " +_player.transform.position);
        }
        else
        {
            Debug.LogWarning("Invalid spawn position for player.");
        }
    }
    void SpawnTractor()
    {
        if (tileManager.IsTileAvailable(_tractorStartPosition))
        {
            Vector2 spawnPosition = new Vector2(_tractorStartPosition.x * tileManager.TileSize, _tractorStartPosition.y * tileManager.TileSize);
            _tractor = Instantiate(TractorPrefab, spawnPosition, Quaternion.identity);
            //Debug.Log("(TRACTOR): " + _tractor.transform.position);
        }
        else
        {
            Debug.LogWarning("Invalid spawn position for tractor.");
        }
    }
   



    void CurrentTarget() // Indicates which object is the main target.
    {

    }


}
