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

    private GameObject _player;
    private GameObject _tractor;
    private GameObject _cart;

    //All 3 should have their dedicated spawn point once the dictionary works
    private Vector2Int _playerStartPosition = new Vector2Int(5, 5);
    private Vector2Int _tractorStartPosition = new Vector2Int(6, 6);
    private Vector2Int _cartStartPosition = new Vector2Int(0, 0); // Except this this should be one tile behind the tractor at all times probably should be done in cartmvoement script

    public TileManager tileManager;

    // Start is called before the first frame update
    void Start() // Spawns the map player tractor and cart
    {
        tileManager.GenerateGrid();
        SpawnPlayer();
        SpawnTractor();
        SpawnCart();
    }

    void SpawnPlayer()
    {
        if (tileManager.IsTileAvailable(_playerStartPosition))
        {
            Vector2 spawnPosition = new Vector2(_playerStartPosition.x * tileManager.TileSize, _playerStartPosition.y * tileManager.TileSize);
            _player = Instantiate(PlayerPrefab, spawnPosition, Quaternion.identity);
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
}
