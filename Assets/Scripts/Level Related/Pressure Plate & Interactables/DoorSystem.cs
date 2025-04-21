using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    [SerializeField] private GameObject DoorIsUp;   // The door object
    [SerializeField] private GameObject DoorIsDown;   // The door object
    private bool isDoorOpen = false;
    private float currentWeight;

    public void UpdateDoorState(float totalWeight)
    {
        currentWeight = totalWeight;
        FindObjectOfType<GameManager>().DisableInputTemporarily(0.2f);

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
            DoorIsUp.SetActive(false); // Hide door to simulate opening
            DoorIsDown.SetActive(true);
        }
    }

    private void DoorClose()
    {
        if (isDoorOpen)
        {
            isDoorOpen = false;
            DoorIsUp.SetActive(true); // Show door to simulate closing
            DoorIsDown.SetActive(false);
        }
    }
}
