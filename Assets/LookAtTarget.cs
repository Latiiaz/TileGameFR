using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public float actionSpeed = 5f;
    public LayerMask obstacleLayer; // Set this in the Inspector to detect obstacles

    void Update()
    {
        LookAt();
    }

    void LookAt()
    {  GameObject PlayerPressurePlate = GameObject.Find("Player PressurePlate");
        if (PlayerPressurePlate != null)
        {
            Vector3 targetDirection = PlayerPressurePlate.transform.position - transform.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg; // Get angle in degrees

            // Raycast to check for obstacles
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection.normalized, targetDirection.magnitude, obstacleLayer);

            if (hit.collider == null) // No obstacle detected
            {
                float smoothAngle = Mathf.LerpAngle(transform.eulerAngles.z, angle, actionSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 0, smoothAngle); // Apply rotation in 2D
            }

            // Debugging: Draw ray in Scene View
            Debug.DrawRay(transform.position, targetDirection.normalized * targetDirection.magnitude, hit.collider == null ? Color.green : Color.red);
           
        }
        Debug.LogWarning("PlayerPressurePlate is not assigned!");
        return;
    }
}
