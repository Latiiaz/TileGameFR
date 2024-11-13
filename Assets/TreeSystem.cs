using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSystem : MonoBehaviour, IInteractable
{
    // Tree System and destroying. Need to pair with dictionary to ensure player doesnt travel over it randomly
    // will get "Tractor No" msg but exiting the tractor takes priority and does not destroy game object cuz of that.
    public void Interact() // Need to find a way to "destroy" the trees cuz E exits the tractor now >:(
    {
        if (IsTractor())
        {
            Debug.Log("Tractor Yes");
            Destroy(gameObject); 
        }
        else
        {
            Debug.Log("Tractor No");
        }
    }

    public bool IsTractor()
    {
        return gameObject.CompareTag("Tractor");
    }
}
