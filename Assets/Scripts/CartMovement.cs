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
        lastTractorPosition = tractor.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (tractor.position != lastTractorPosition)
        {
            Vector3 targetCartPosition = tractor.position + offsetBehindTractor;

            transform.position = targetCartPosition;

            lastTractorPosition = tractor.position;
            transform.rotation = tractor.rotation;
        }
    }
}
