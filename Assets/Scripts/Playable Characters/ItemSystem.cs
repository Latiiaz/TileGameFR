using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSystem : MonoBehaviour
{
    ////Same as tractor script to handle "carrying" 
    //// Tether system to the player
    //// Inability to move onto certain tiles such as the river

    //[SerializeField] private TileManager _tileManager;
    //private Vector2Int _itemPosition = new Vector2Int(0, 0);

    //private PlayerMovement _player;
    //private ObjectiveSystem _objectiveSystem;

    //public bool IsHidden { get; private set; } = false;

    //[SerializeField] private float weight;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    _tileManager = FindObjectOfType<TileManager>();
    //    _player = FindObjectOfType<PlayerMovement>();
    //    _objectiveSystem = FindObjectOfType<ObjectiveSystem>();
        
    //}

    //public void InteractE()
    //{
    //    // Ensure the player has an InventorySystem component
    //    InventorySystem inventory = _player.GetComponent<InventorySystem>();

    //    if (inventory != null)
    //    {
    //        if (!inventory.IsHoldingItem())
    //        {
    //            inventory.PickUpItem(this);
    //        }
    //        else
    //        {
    //            Debug.Log("Player is already holding an item.");
    //        }
    //    }
    //}
    //public void Hide()
    //{
    //    IsHidden = true;
    //    gameObject.SetActive(false); 
    //    Debug.Log($"{gameObject.name} is now hidden.");
    //}

    ////public void Reveal(Vector3 position)
    ////{
    ////    IsHidden = false;
    ////    gameObject.SetActive(true);
    ////    transform.position = position + new Vector3(_player._playerDirection.x, _player._playerDirection.y, 0);
    ////    Debug.Log($"{gameObject.name} is now visible at {position}.");
    ////}

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    GameObject _cart = GameObject.FindWithTag("Cart");
    //    if (other != _cart)
    //    {
           
    //    }
    //    else 
    //    {
    //        Debug.Log("dwanjid");
    //        _objectiveSystem._objectiveCount++;
    //        Destroy(gameObject);
    //    }
    //}

    //public float GetWeight()
    //{
    //    return weight;
    //}
}