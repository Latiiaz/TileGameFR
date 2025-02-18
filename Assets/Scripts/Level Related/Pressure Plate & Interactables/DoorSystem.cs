using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    [SerializeField] private GameObject nonCodeObject;   // The door object
    [SerializeField] private float requiredWeight = 50f; // The weight required to open the door
    [SerializeField] private bool isDoorOpen = false;


    private void Update()
    {
    }
    public void UpdateDoorState(float totalWeight)
    {
        if (totalWeight == (requiredWeight) && !isDoorOpen)
        {
            DoorOpen();
        }
        else if (totalWeight != requiredWeight && isDoorOpen)
        {
            DoorClose();
        }
    }

    private void DoorOpen()
    {
        Debug.Log("Required weight achieved. The door is now open!");
        isDoorOpen = true;
        nonCodeObject.SetActive(false); // Hide the door to simulate it opening
    }

    private void DoorClose()
    {
        Debug.Log("Required weight not met. The door is now closed!");
        isDoorOpen = false;
        nonCodeObject.SetActive(true); // Show the door to simulate it closing
    }
}
