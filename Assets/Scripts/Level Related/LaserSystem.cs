using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSystem : MonoBehaviour
{
    public float laserRange = 10f;
    public float laserOffset = 0.1f;
    public Vector2 laserDirection = Vector2.right;
    public LineRenderer lineRenderer;

    [Header("Layer Masks")]
    public LayerMask passThroughLayers;
    public LayerMask stopLayers;
    public LayerMask hideLayers;

    [Header("Particle Effects")]
    public GameObject playerHitEffect;
    public GameObject tractorHitEffect;

    private List<Transform> hitObjects = new List<Transform>();
    private List<Vector2> hitPoints = new List<Vector2>();

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

        Vector2 startPosition = (Vector2)transform.position + laserDirection.normalized * laserOffset;
        Vector2 direction = transform.TransformDirection(laserDirection).normalized;
        Vector2 currentPosition = startPosition;

        hitPoints.Add(currentPosition);
        float remainingDistance = laserRange;
        int maxBounces = 10;
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

                // Check tag and spawn corresponding particle effect
                // Only play particle if object is on hideLayers
                if (((1 << hit.collider.gameObject.layer) & hideLayers) != 0)
                {
                    // Play specific particle depending on tag
                    if (hit.collider.CompareTag("Player") && playerHitEffect != null)
                    {
                        Instantiate(playerHitEffect, hit.point, Quaternion.identity);
                    }
                    else if (hit.collider.CompareTag("Tractor") && tractorHitEffect != null)
                    {
                        Instantiate(tractorHitEffect, hit.point, Quaternion.identity);
                    }

                    // Hide the object
                    hit.collider.gameObject.SetActive(false);
                }


                // Hide logic
                if (((1 << hit.collider.gameObject.layer) & hideLayers) != 0)
                {
                    hit.collider.gameObject.SetActive(false);
                }

                // Stop logic
                if (((1 << hit.collider.gameObject.layer) & stopLayers) != 0)
                {
                    break;
                }

                // Continue if passthrough
                if (((1 << hit.collider.gameObject.layer) & passThroughLayers) != 0)
                {
                    bounces++;
                    continue;
                }
            }
            else
            {
                hitPoints.Add(currentPosition + direction * remainingDistance);
                break;
            }
        }

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
