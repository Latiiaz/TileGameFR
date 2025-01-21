using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherCircleIndicator : MonoBehaviour
{
    [SerializeField] private TetherSystem _tetherSystem; 
    private SpriteRenderer _spriteRenderer; 

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(FindTetherSystemOnPlayer());
    }

    void Update()
    {
        if (_tetherSystem == null) return;

        float newScale = _tetherSystem.maxSteps * 2;
        transform.localScale = new Vector3(newScale, newScale, 1f);

        // Update visibility based on the tether's activation state
        _spriteRenderer.enabled = _tetherSystem.IsCurrentlyActive;
    }

    private IEnumerator FindTetherSystemOnPlayer()
    {
        // Wait until the player and tractor are found
        while (GameObject.FindWithTag("Player") == null)
        {
            yield return null;
        }

        GameObject tractor = GameObject.FindWithTag("Tractor");
        if (tractor != null)
        {
            _tetherSystem = tractor.GetComponent<TetherSystem>();
        }
        else
        {
            Debug.LogWarning("Tractor with TetherSystem not found!");
        }
    }
}
