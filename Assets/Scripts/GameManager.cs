using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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
    private bool _enemyTurn = false;

    // Start is called before the first frame update
    void Start() // Spawns the map player tractor and cart
    {
        tileManager.GenerateGrid(); // Spawns grids so all tiles have spawned
        
        SpawnTractor(); // Spawn tractor since the player needs to find the tractor in order to spawn on it
        // The player spawns inside of the tractor and the F tractor key is called on start for the player, hardcoded way to always start in the tractor
        SpawnCart(); // Cart needs to spawn behind the tractor hence spawns after the player and tractor are both done spawning
        SpawnItem(); // Completely unaffected by the spawn conditions of the other 3 above it
        SpawnPylon();
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
        if (Input.GetKeyDown(KeyCode.F))
        {

            SwitchTurn();
        }
    }

    public void SwitchTurn()
    {
        _currentTurnIsPlayer = !_currentTurnIsPlayer;
        TurnChecker();
    }

    public void TurnChecker()
    {

        if (_currentTurnIsPlayer)
        {
            _enemyTurn = false;
            //Debug.Log("It's now the Player's turn.");
        }
        else
        {
            _enemyTurn = true;
            //Debug.Log("It's now the Enemy's turn.");
        }
    }

    public bool TurnStatus()
    {
        return _currentTurnIsPlayer;
    }


    void SpawnPlayer()
    {
        if (tileManager.IsTileAvailable(_tractorStartPosition))
        {
            Debug.Log("(PLAYER SPAWN LOCATION): " + _tractorStartPosition);
            Vector2 spawnPosition = new Vector2(_tractorStartPosition.x * tileManager.TileSize, _tractorStartPosition.y * tileManager.TileSize);
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
    void SpawnCart()
    {
        if (tileManager.IsTileAvailable(_cartStartPosition))
        {
            Vector2 spawnPosition = new Vector2(_cartStartPosition.x * tileManager.TileSize, _cartStartPosition.y * tileManager.TileSize);
            _cart = Instantiate(CartPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Invalid spawn position for cart.");
        }
    }

    void SpawnItem()
    {
        if (tileManager.IsTileAvailable(_itemTestStartPosition))
        {
            Vector2 spawnPosition = new Vector2(_itemTestStartPosition.x * tileManager.TileSize, _itemTestStartPosition.y * tileManager.TileSize);
            _itemTest = Instantiate(ItemTestPrefab, spawnPosition, Quaternion.identity);
            //Debug.Log("(TEST ITEM): " + _itemTest.transform.position);
        }
        else
        {
            Debug.LogWarning("Invalid spawn position for test item.");
        }
    }
    void SpawnPylon()
    {
        if (tileManager.IsTileAvailable(_pylonStartPosition))
        {
            Vector2 spawnPosition = new Vector2(_pylonStartPosition.x * tileManager.TileSize, _pylonStartPosition.y * tileManager.TileSize);
            _pylon = Instantiate(PylonPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Invalid spawn position for cart.");
        }
    }



    void CurrentTarget() // Indicates which object is the main target.
    {

    }


}
