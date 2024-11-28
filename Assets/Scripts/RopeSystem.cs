using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSystem : MonoBehaviour, IInteractable
{
    // Rope System to give the player more lee-way when moving in the tractor movement circle. 
    // Draw line to indicate the player + tractor tether

    [SerializeField] TetherSystem _tetherSystem;

    void Awake()
    {
        StartCoroutine(FindTetherSystemOnPlayer());
    }

    public void InteractE()
    {
        Debug.Log("E key works");
        _tetherSystem.CollectRopeItem();
        Destroy(gameObject);

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
