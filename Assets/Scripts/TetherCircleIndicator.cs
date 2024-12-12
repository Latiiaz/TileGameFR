using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherCircleIndicator : MonoBehaviour
{
    [SerializeField] private TetherSystem _tetherSystem;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    void Awake()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        StartCoroutine(FindTetherSystemOnPlayer());
    }

    void Update()
    {
        if (_tetherSystem == null)
        {
            _spriteRenderer.enabled = false;
        }
        float newScale = _tetherSystem.maxSteps * 2;
        transform.localScale = new Vector3(newScale, newScale, 1f);
        _spriteRenderer.enabled = _tetherSystem.IsCurrentlyActive;
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

<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("It do the thing");
    }
}
