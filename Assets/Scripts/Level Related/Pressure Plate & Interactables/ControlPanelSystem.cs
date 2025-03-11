using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ControlPanelSystem : MonoBehaviour, IInteractable
{
    [SerializeField] private PressurePlateSystem pressurePlate;
    public void InteractE()
    {
        if (pressurePlate != null)
        {
            //pressurePlate.InteractE();
        }
    }
}
