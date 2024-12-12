using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenTimerSystem : MonoBehaviour
{
    [SerializeField] public float oxygenTime = 60f; // Oxygen timer in seconds before player faints
    private float currentTime;

    [SerializeField] public LevelManager levelManager;
    private List<TetherSystem> tetherSystems;

    void Start()
    {
        currentTime = oxygenTime;
        levelManager = FindAnyObjectByType<LevelManager>();

        // Find all active TetherSystem components in the scene
        tetherSystems = new List<TetherSystem>(FindObjectsOfType<TetherSystem>());
    }

    void FixedUpdate()
    {
        if (currentTime > 0)
        {
            float depletionRate = IsPlayerInsideTether() ? Time.deltaTime : Time.deltaTime * 5;
            currentTime -= depletionRate;
            currentTime = Mathf.Max(currentTime, 0);

            //DisplayTime();
        }
        else
        {
            levelManager.LoadVictoryScene();
            Debug.Log("Oxygen bar is out, player faints.");
        }
    }

    private bool IsPlayerInsideTether()
    {
        foreach (var tether in tetherSystems)
        {
            if (tether.IsCurrentlyActive)
            {
                Vector2 playerPosition = transform.position; // Assume this script is attached to the player
                Vector2 tetherPosition = tether.transform.position;
                float distance = Vector2.Distance(playerPosition, tetherPosition);

                if (distance <= tether.maxSteps)
                {
                    return true; // Player is inside at least one active tether range
                }
            }
        }
        return false; // Player is outside all active tethers
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
