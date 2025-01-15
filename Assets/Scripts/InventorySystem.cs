using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] public GameObject heldItem = null;

    public bool IsHoldingItem()
    {
        return heldItem != null;
    }

    //public bool IsHoldingSpecificItem(ItemSystem item)
    //{
    //    return heldItem != null && heldItem.GetComponent<ItemSystem>() == item;
    //}

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E) && IsHoldingItem()) // Pick up items but it drops the 
        {
            DropItem();
        }
    }


    public GameObject GetHeldItem()
    {
        return heldItem;
    }

    public void PickUpItem(ItemSystem item)
    {
        if (item == null || IsHoldingItem()) return;

        heldItem = item.gameObject;
        item.Hide(); // Hide the item using ItemSystem
        Debug.Log($"Picked up item: {heldItem.name}");
    }
    public void DropItem()
    {
        GameObject _cart = GameObject.FindWithTag("Cart");
        if (heldItem != null)
        {
            if (_cart == null) // Supposed to be the script for depositing the item in the cart
            {
                Debug.Log($"Dropped item: {heldItem.name}");
                ItemSystem itemSystem = heldItem.GetComponent<ItemSystem>();
                if (itemSystem != null)
                {
                    Vector3 playerPosition = transform.position;
                    itemSystem.Reveal(playerPosition);
                }
                heldItem = null;
            }
            else
            {
               
            }
            
        }
    }
}
