using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    [SerializeField] private GameObject nonCodeObject;   // The door object
    private bool isDoorOpen = false;
    private float currentWeight;

    public void UpdateDoorState(float totalWeight)
    {
        currentWeight = totalWeight;

        // Open the door if the weight is NOT divisible by 100
        if (currentWeight % 100 == 0)
        {
            DoorClose();
        }
        else
        {
            DoorOpen();
        }
    }

    private void DoorOpen()
    {
        if (!isDoorOpen)
        {
            isDoorOpen = true;
            nonCodeObject.SetActive(false); // Hide door to simulate opening
        }
    }

    private void DoorClose()
    {
        if (isDoorOpen)
        {
            isDoorOpen = false;
            nonCodeObject.SetActive(true); // Show door to simulate closing
        }
    }
}
