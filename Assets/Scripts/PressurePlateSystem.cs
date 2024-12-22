using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateSystem : MonoBehaviour
{
    [SerializeField] public bool output;

    public bool GetOutputStatus()
    {
        return output;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ToggleOutput();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        ToggleOutput();
    }

    private void ToggleOutput()
    {
        output = !output;
        Debug.Log($"Pressure Plate Output Changed: {output}");
    }
}