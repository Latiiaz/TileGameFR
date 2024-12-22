using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    public GameObject[] PressurePlates;
    public GameObject NonCodeObject;
    [SerializeField] private bool isDoorOpen = false;


    void Start()
    {
        
    }
    void Update()
    {

        CheckPressurePlates();
    }


    public void CheckPressurePlates()
    {

        bool allPlatesActive = true;

        for (int i = 0; i < PressurePlates.Length; i++)
        {
            GameObject pressureplate = PressurePlates[i];
            if (pressureplate.CompareTag("PressurePlate"))
            {
                PressurePlateSystem plateSystem = pressureplate.GetComponent<PressurePlateSystem>();
                if (plateSystem != null)
                {
                    bool plateOutput = plateSystem.GetOutputStatus();

                    if (!plateOutput)
                    {
                        allPlatesActive = false;
                    }
                }

            }
            else
            {
                allPlatesActive = false;
            }
        }

        if (allPlatesActive && !isDoorOpen)
        {
            DoorOpen();
        }
        else if (!allPlatesActive && isDoorOpen)
        {
            DoorClose();
        }
    }

    private void DoorOpen()
    {
        Debug.Log("All pressure plates are active. The door is now open!");
        isDoorOpen = true;

        NonCodeObject.SetActive(false);
    }

   
    private void DoorClose()
    {
        Debug.Log("Not all pressure plates are active. The door is now closed!");
        isDoorOpen = false;

        NonCodeObject.SetActive(true);
    }
}
