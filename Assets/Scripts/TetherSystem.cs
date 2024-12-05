using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherSystem : MonoBehaviour
{
    [SerializeField] Transform _tractorpos;
    [SerializeField] public int maxSteps = 4; // Circle indication on tractor tether is scale = 2 *max steps
    [SerializeField] private int currentSteps;
    [SerializeField] private Vector2 lastPlayerPosition;  
    [SerializeField] private Transform playerTransform;

    public bool CanMove
    {
        get { return currentSteps < maxSteps; }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject _newTractor = GameObject.FindWithTag("Tractor");
        _tractorpos = _newTractor.transform;

        playerTransform = GetComponent<Transform>();

        if (_tractorpos == null)
        {
            Debug.LogWarning("Tractor reference is missing in TetherSystem!");
            return;
        }

        if (playerTransform == null)
        {
            Debug.LogWarning("Player's transform not found!");
            return;
        }
        lastPlayerPosition = playerTransform.position;
        currentSteps = 0;

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 tractorPosition = _tractorpos.transform.position;
        Vector2 playerPosition = playerTransform.position;

       
        if (playerPosition != lastPlayerPosition)
        {
            UpdateSteps(playerPosition, tractorPosition);
            lastPlayerPosition = playerPosition; 
        }

      
        if (currentSteps >= maxSteps)
        {
            //Debug.Log("Player has reached the maximum tether range!");
            // Link to playermovement script
        }

    }

    private void UpdateSteps(Vector2 playerPosition, Vector2 tractorPosition)
    {
        int distanceBefore = Mathf.RoundToInt(Vector2.Distance(lastPlayerPosition, tractorPosition));
        int distanceAfter = Mathf.RoundToInt(Vector2.Distance(playerPosition, tractorPosition));

        if (distanceAfter > distanceBefore) // Player moving away from the pivot point (tractor)
        {
            currentSteps++;
        }
        else if (distanceAfter < distanceBefore) // Player moving towards the pivot point
        {
            currentSteps--;
        }


        currentSteps = Mathf.Clamp(currentSteps, 0, maxSteps);

       // Debug.Log($"Current Steps: {currentSteps}, Max Steps: {maxSteps}");
    }
    public bool CanMoveTowardTractor(Vector2 playerMovement)
    {
        float distanceToTractor = Vector2.Distance(playerTransform.position, _tractorpos.position);

        if (distanceToTractor >= maxSteps)
        {
            Vector2 movementDirection = playerMovement.normalized;
            Vector2 directionToTractor = (_tractorpos.position - playerTransform.position).normalized;
          
            if (Vector2.Dot(movementDirection, directionToTractor) < 0)
            {
                return false;
            }
        }
        return true;
    }

    public void CollectRopeItem()
    {
        maxSteps += 2;
    }

    public int GetMaxSteps()
    {
        return maxSteps;
    }
}
