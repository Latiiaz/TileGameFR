using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveSystem : MonoBehaviour 
    // Score System
{
    public LevelManager levelManager;

    public int _objectiveCount;
    public int _neededObjectives;

    private float _victoryDelay = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (_objectiveCount >= _neededObjectives)
        {
            VictorySequence();
        }
    }

    void VictorySequence() // Move the tractor out, player walks back into the tractor following the tether line
    {
        StartCoroutine(WaitForSecondsCoroutine(_victoryDelay));
        
    }

    private IEnumerator WaitForSecondsCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        levelManager.LoadVictoryScene();

    }
}
