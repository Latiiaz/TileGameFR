using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    //For all canvas and UI stuff
    // E/F Interact Keys, Maximum Steps, WASD keys on the side too?

    [SerializeField] GameObject _EInteract;
    [SerializeField] GameObject _FInteract;

    [SerializeField] TextMeshProUGUI _maxDistanceTracker;
    TetherSystem _tetherSystem;


    [SerializeField] Slider oxygenBar;
    [SerializeField] OxygenTimerSystem _oxygenTimerSystem;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FindTetherSystemOnPlayer());

        oxygenBar.maxValue = _oxygenTimerSystem.oxygenTime;
        oxygenBar.value = _oxygenTimerSystem.GetTimeRemaining();

    }

    // Update is called once per frame
    void Update()
    {
        _maxDistanceTracker.text = _tetherSystem.GetMaxSteps().ToString("000");
        oxygenBar.value = _oxygenTimerSystem.GetTimeRemaining();
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

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            _tetherSystem = player.GetComponent<TetherSystem>();
        }

    }

}
