using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenTimerSystem : MonoBehaviour
{
    [SerializeField] public float oxygenTime = 60f; // Oxygen timer in seconds before player faints // if player is outside of the tether range, they lose oxygen much faster
    private float currentTime;

    [SerializeField] public LevelManager levelManager;

    void Start()
    {
        currentTime = oxygenTime;
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime; 
            currentTime = Mathf.Max(currentTime, 0); 

            //DisplayTime();
        }
        else
        {
            levelManager.LoadVictoryScene();
            Debug.Log("Oyxgen Bar is out, player faints");
        }
    }

    private void DisplayTime()
    {
        Debug.Log($"Time Remaining: {currentTime:00}");
    }

    public void SetCountdownTime(float newTime)
    {
        oxygenTime = newTime;
        currentTime = oxygenTime;
    }

    public float GetTimeRemaining()
    {
        return currentTime;
    }
}
