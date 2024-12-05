using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherCircleIndicator : MonoBehaviour
{
    [SerializeField] TetherSystem _tetherSystem;


    void Awake()
    {
        StartCoroutine(FindTetherSystemOnPlayer());
    }
    private void Update()
    {
        float newScale = _tetherSystem.maxSteps * 2;

        transform.localScale = new Vector3(newScale, newScale, 1f);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("It do the thing");
    }
}
