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

        SetItemSpawnPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.IsCarryingItem)
        {
            
        }
    }


    void SetItemSpawnPosition() // Item spawn position
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


   

    IEnumerator ActionCooldown()
    {
        yield return new WaitForSeconds(_actionCooldown);
        _isActionOnCooldown = false;
    }

    public void InteractE() //Enter Exit tractor
    {
        Debug.Log("clickey");
        // TRACKTOR BRAINS!!!!!! (move the player inside)
        if (_player.IsCarryingItem)
        {
            _player.AttemptCarryOrDropItem(); // Swap to InventorySystem later on 
        }
        else
        {
            _player.AttemptCarryOrDropItem();
        }
        Destroy(gameObject);
    }
    public Vector2Int GetTractorPosition() // Can remove eventually when implemented inventory system
    {
        return _tractorPosition;
    }

    void OnCollisionStay2D(Collision2D collision) // interacting with the cart
    {
        if (_player.IsCarryingItem && collision.gameObject.CompareTag("Cart"))
        {
            _player.AttemptCarryOrDropItem();
            Destroy(this.gameObject);
            objectiveSystem._objectiveCount++;
            Debug.Log("Item Deposited, Victory Sequence");
        }
    }
}
