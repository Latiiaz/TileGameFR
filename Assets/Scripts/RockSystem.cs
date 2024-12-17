using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSystem : MonoBehaviour, IInteractable
{
    public void InteractE() // Need to find a way to "destroy" the trees cuz E exits the tractor now >:(
    {
        Debug.Log("E key works");
        Destroy(gameObject);
    }
}
