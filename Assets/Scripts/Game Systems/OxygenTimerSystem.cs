using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenTimerSystem : MonoBehaviour
{
    [Header("Oxygen Settings")]
    [SerializeField] public float maxOxygen = 60f; //Supposed number is 30, but increased just to test stuff
    [SerializeField] private float currentOxygen;  
    [SerializeField] private float displayedOxygen; 

    [SerializeField] private LevelManager levelManager;
    private List<TetherSystem> tetherSystems;

    void Start()
    {
        currentOxygen = maxOxygen;
        displayedOxygen = currentOxygen;
        levelManager = FindAnyObjectByType<LevelManager>();

        // Find all active TetherSystem components in the scene
        tetherSystems = new List<TetherSystem>(FindObjectsOfType<TetherSystem>());
    }

    void Update()
    {
        if (currentOxygen > 0)
        {
            float depletionRate;

            if (IsPlayerInsideTether())
            {
                depletionRate = Time.deltaTime;
                currentOxygen += depletionRate * 5; // Replenish oxygen when inside tether
            }
            else
            {
                depletionRate = Time.deltaTime;
                currentOxygen -= depletionRate; // Deplete oxygen faster when outside tether
            }

            currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);
        }
        else
        {
            levelManager.LoadDefeatScene();
            Debug.Log("Oxygen depleted, player faints.");
        }

        displayedOxygen = currentOxygen;

        // Optionally display oxygen in the console
        DisplayOxygen();
    }

    private bool IsPlayerInsideTether()
    {
        foreach (var tether in tetherSystems)
        {
            if (tether.IsCurrentlyActive)
            {
                Vector2 playerPosition = transform.position;
                Vector2 tetherPosition = tether.transform.position;
                float distance = Vector2.Distance(playerPosition, tetherPosition);

                if (distance <= tether.maxSteps)
                {
                    return true; 
                }
            }
        }
        return false;
    }

    private void DisplayOxygen()
    {
        //Debug.Log($"Current Oxygen: {currentOxygen:0.00}");
    }

    public void SetCountdownTime(float newTime)
    {
        maxOxygen = newTime;
        currentOxygen = maxOxygen;
    }

    public float GetTimeRemaining()
    {
        return currentOxygen;
    }
}
