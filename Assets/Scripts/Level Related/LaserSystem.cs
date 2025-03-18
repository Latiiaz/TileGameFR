using System.Collections.Generic;
using UnityEngine;

public class LaserSystem : MonoBehaviour
{
    public float laserRange = 10f;
    public float laserOffset = 0.1f; // Adjustable offset
    public Vector2 laserDirection = Vector2.right;
    public LineRenderer lineRenderer;

    [Header("Layer Masks")]
    public LayerMask passThroughLayers; // Layers the laser can pass through
    public LayerMask stopLayers; // Layers that stop the laser
    public LayerMask hideLayers; // Layers that the laser will hide

    private List<Transform> hitObjects = new List<Transform>(); // Objects hit by the laser
    private List<Vector2> hitPoints = new List<Vector2>(); // Points where laser hits

    void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        if (lineRenderer != null)
        {
            lineRenderer.sortingLayerName = "Foreground";
            lineRenderer.sortingOrder = 10;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }
    }

    void Update()
    {
        ShootLaser();
    }

    void ShootLaser()
    {
        hitObjects.Clear();
        hitPoints.Clear();

        Vector2 startPosition = (Vector2)transform.position + laserDirection.normalized * laserOffset; // Uses offset
        Vector2 direction = transform.TransformDirection(laserDirection).normalized;
        Vector2 currentPosition = startPosition;

        hitPoints.Add(currentPosition); // Start point of the laser

        float remainingDistance = laserRange;
        int maxBounces = 10; // Prevent infinite loops
        int bounces = 0;

        while (remainingDistance > 0 && bounces < maxBounces)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPosition, direction, remainingDistance, passThroughLayers | stopLayers | hideLayers);

            if (hit.collider != null)
            {
                hitObjects.Add(hit.collider.transform);
                hitPoints.Add(hit.point);

                float distanceToHit = Vector2.Distance(currentPosition, hit.point);
                remainingDistance -= distanceToHit;
                currentPosition = hit.point;

                // Check if the object is in the hideLayers and disable it
                if (((1 << hit.collider.gameObject.layer) & hideLayers) != 0)
                {
                    hit.collider.gameObject.SetActive(false);
                    Debug.Log($"Laser hit and disabled {hit.collider.gameObject.name} at {hit.point}");
                }

                // If the object is in the stopLayers, stop the laser
                if (((1 << hit.collider.gameObject.layer) & stopLayers) != 0)
                {
                    Debug.Log($"Laser stopped by {hit.collider.gameObject.name}");
                    break;
                }

                // If the object is in passThroughLayers, continue the laser
                if (((1 << hit.collider.gameObject.layer) & passThroughLayers) != 0)
                {
                    Debug.Log($"Laser passed through {hit.collider.gameObject.name}");
                    bounces++; // Prevent infinite loop
                    continue;
                }
            }
            else
            {
                // No hit, extend to max range
                hitPoints.Add(currentPosition + direction * remainingDistance);
                break;
            }
        }

        // Update LineRenderer
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = hitPoints.Count;
            for (int i = 0; i < hitPoints.Count; i++)
            {
                lineRenderer.SetPosition(i, new Vector3(hitPoints[i].x, hitPoints[i].y, -0.1f));
            }
        }
    }

    public List<Transform> GetHitObjects()
    {
        return new List<Transform>(hitObjects);
    }
}
