using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBoxSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }







    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            Debug.Log($"The InteractBox is now targeting: {collision.gameObject.name}");
        }
    }
    private void OnTriggerStay2D(Collider2D collision) // On Stay
    {
        //if (collision != null)
        //{
        //    Debug.Log($"The InteractBox is now targeting: {collision.gameObject.name}");
        //}
    }
    private void OnTriggerExit2D(Collider2D collision) // On Exit
    {
        //if (collision != null)
        //{
        //    Debug.Log($"The InteractBox is now targeting: {collision.gameObject.name}");
        //}
    }
}
