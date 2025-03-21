using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{

    // Handles most On Screen UI instead of worldspace UI
    //For all canvas and UI stuff
    // E/F Interact Keys, Maximum Steps, WASD keys on the side too?

    [SerializeField] GameObject _EInteract;
    [SerializeField] GameObject _FInteract;

    [SerializeField] TextMeshProUGUI _maxDistanceTracker;
    TetherSystem _tetherSystem;


    [SerializeField] Slider oxygenBar;
    [SerializeField] OxygenTimerSystem _oxygenTimerSystem;


    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(FindTetherSystemOnPlayer());

        oxygenBar.maxValue = _oxygenTimerSystem.maxOxygen;
        oxygenBar.value = _oxygenTimerSystem.GetTimeRemaining(); //done in Update. Unless want to make it so that the player does not lose oxygen

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void ShowEInteract(bool activateE)
    {
        if (true)
        {
            _EInteract.SetActive(activateE);
        }
    }

    public void ShowFInteract(bool activateF)
    {
        if (true)
        {
            _FInteract.SetActive(activateF);
        }
    }



    private IEnumerator FindTetherSystemOnPlayer()
    {
        while (GameObject.FindWithTag("Player") == null)
        {
            yield return null;
        }

        GameObject tractor = GameObject.FindWithTag("Tractor");
        if (tractor != null)
        {
            _tetherSystem = tractor.GetComponent<TetherSystem>();
        }

    }

}
