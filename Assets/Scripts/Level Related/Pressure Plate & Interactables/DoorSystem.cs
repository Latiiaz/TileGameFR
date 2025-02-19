using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    [SerializeField] private GameObject nonCodeObject;   // The door object
    [SerializeField] private bool isDoorOpen = false;
    [SerializeField] private float currentWeight;


    private void Update()
    {
        currentWeight = CalculateTotalWeight();
        if (currentWeight % 100 == 0)
        {
            DoorClose();
        }
        else
        {
            DoorOpen();
        }
    }
    public void UpdateDoorState(float totalWeight)
    {
        float addedweight = totalWeight;
        currentWeight = addedweight;
    }


    private void DoorOpen()
    {
        //Debug.Log("Required weight achieved. The door is now open!");
        isDoorOpen = true;
        nonCodeObject.SetActive(false); // Hide the door to simulate it opening
    }

    private void DoorClose()
    {
        //Debug.Log("Required weight not met. The door is now closed!");
        isDoorOpen = false;
        nonCodeObject.SetActive(true); // Show the door to simulate it closing
    }


    private float CalculateTotalWeight()
    {
        float totalWeight = 0f;
        PressurePlateSystem[] pressurePlates = FindObjectsOfType<PressurePlateSystem>();

        foreach (PressurePlateSystem plate in pressurePlates)
        {
            // Check if this door is the currently controlled door for the pressure plate
            if (plate.GetCurrentDoor() == this)
            {
                totalWeight += plate.GetTotalWeight();
            }
        }

        return totalWeight;
    }
}
