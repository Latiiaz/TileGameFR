using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalPickupSystem : MonoBehaviour
{
    private PlayerMovement _player;
    private ObjectiveSystem _objectiveSystem;



    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerMovement>();
        _objectiveSystem = FindObjectOfType<ObjectiveSystem>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject _player = GameObject.FindWithTag("Player");
        GameObject _robot = GameObject.FindWithTag("Tractor");

        if (other == _player || _robot)
        {
            _objectiveSystem._objectiveCount++;
            Destroy(gameObject);
        }
        //Debug.Log("dwanjid");
      
    }
}
