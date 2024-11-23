using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartMovement : MonoBehaviour
{
    // Cart Movement
    // Maybe store the movement of the tractor and the cart is always one step behind it and the first move the cart does is to always move up? followed by the actions of the tractor
    // More than 1 cart can spawn maybe if wanna add more than 1 objective to beating the level

    public Transform tractor; 
    public Vector3 offsetBehindTractor = new Vector3(0, -1,0);  // -1 spawns it rigth behind the tractor
    private Vector3 lastTractorPosition;

  

    // Start is called before the first frame update
    void Start()
    {
        // Find the tractor GameObject by tag
        GameObject tractorObject = GameObject.FindWithTag("Tractor");
        if (tractorObject != null)
        {
            tractor = tractorObject.transform;
            lastTractorPosition = tractor.position;
        }
        else
        {
            Debug.LogError("CartMovement: No GameObject with the tag 'Tractor' was found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Ensure the tractor exists before proceeding
        if (tractor == null)
        {
            Debug.LogWarning("CartMovement: Tractor reference is missing.");
            return;
        }

        // Update the cart's position and rotation if the tractor has moved
        if (tractor.position != lastTractorPosition)
        {
            Vector3 targetCartPosition = tractor.position + offsetBehindTractor;
            transform.position = targetCartPosition;

            lastTractorPosition = tractor.position;
            transform.rotation = tractor.rotation;
        }
    }
}
