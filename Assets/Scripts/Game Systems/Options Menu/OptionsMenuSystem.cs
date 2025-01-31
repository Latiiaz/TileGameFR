using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuSystem : MonoBehaviour
{
    public GameObject optionsMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            optionsMenu.SetActive(!optionsMenu.activeSelf);
        }
    }
}
