using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    public GameObject[] PressurePlates; // Assign connected pressure plates in the Inspector
    public GameObject NonCodeObject;   // The door object
    [SerializeField] private bool isDoorOpen = false;

    [SerializeField] private float requiredWeight = 50f; // The weight required to open the door

    void Update()
    {
        CheckPressurePlates();
    }

    private void CheckPressurePlates()
    {
        float totalWeight = 0f;

        // Calculate the total weight from all connected pressure plates
        foreach (GameObject pressurePlate in PressurePlates)
        {
            if (pressurePlate.CompareTag("PressurePlate"))
            {
                PressurePlateSystem plateSystem = pressurePlate.GetComponent<PressurePlateSystem>();
                if (plateSystem != null)
                {
                    totalWeight += plateSystem.GetTotalWeight();
                }
            }
        }

        Debug.Log($"Total Weight on Plates: {totalWeight}");

        // Check if the total weight meets the required weight to open the door
        if (totalWeight == requiredWeight && !isDoorOpen)
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

        NonCodeObject.SetActive(false); // Hide the door to simulate it opening
    }

    private void DoorClose()
    {
        Debug.Log("Required weight not met. The door is now closed!");
        isDoorOpen = false;

        NonCodeObject.SetActive(true); // Show the door to simulate it closing
    }
}
