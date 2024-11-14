using System.Collections.Generic;
using UnityEngine;

public class TreeSystem : MonoBehaviour, IInteractable
{
    // Tree System and destroying. Need to pair with dictionary to ensure player doesnt travel over it randomly
    // will get "Tractor No" msg but exiting the tractor takes priority and does not destroy game object cuz of that.
    public void InteractE() // Need to find a way to "destroy" the trees cuz E exits the tractor now >:(
    {
        Debug.Log("E key works");
        Destroy(gameObject);

        if (IsTractor()) // Is tractor check doesnt work
        {
            Debug.Log("Tractor Yes");
            Destroy(gameObject); 
        }
        else
        {
            Debug.Log("Needs to be Tractor not Player");
        }
    }

    public bool IsTractor()
    {
        return gameObject.CompareTag("Tractor");
    }
}
