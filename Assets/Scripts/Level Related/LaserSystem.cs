using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSystem : MonoBehaviour
{
    public float laserRange = 10f;
    public float laserOffset = 0.1f;
    public Vector2 laserDirection = Vector2.right;
    public LineRenderer lineRenderer;
    public CameraManager cameraManager;

    [Header("Layer Masks")]
    public LayerMask passThroughLayers;
    public LayerMask stopLayers;
    public LayerMask hideLayers;

    [Header("Particle Effects")]
    public GameObject playerHitEffect;
    public GameObject tractorHitEffect;

    private List<Transform> hitObjects = new List<Transform>();
    private List<Vector2> hitPoints = new List<Vector2>();

    [Header("Audio")]
    public AudioSource laserLoopSource;
    public AudioClip hideSoundEffect;

    [Header("Laser Pitch Oscillation")]
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;
    public float pitchLerpDuration = 2f;

    private bool laserPaused = false;
    [SerializeField] private GameObject laserEndEffect;

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

        if (laserLoopSource != null && !laserLoopSource.isPlaying)
        {
            laserLoopSource.loop = true;
            laserLoopSource.Play();
        }

        StartCoroutine(PitchLerpLoop());
        cameraManager = FindObjectOfType<CameraManager>();

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

                if (((1 << hit.collider.gameObject.layer) & hideLayers) != 0)
                {
                    if (hit.collider.CompareTag("Player") && playerHitEffect != null)
                    {
                        ExpressionsManager.TriggerDeathExpressionAll(1f);
                        Instantiate(playerHitEffect, hit.point, Quaternion.identity);

                        CameraManager cam = FindObjectOfType<CameraManager>();
                        if (cam != null)
                            StartCoroutine(cam.ShakeCamera(0f, 1.2f));
                    }
                    else if (hit.collider.CompareTag("Tractor") && tractorHitEffect != null)
                    {
                        ExpressionsManager.TriggerDeathExpressionAll(1f);
                        Instantiate(tractorHitEffect, hit.point, Quaternion.identity);

                        CameraManager cam = FindObjectOfType<CameraManager>();
                        if (cam != null)
                            StartCoroutine(cam.ShakeCamera(0f, 1.2f));
                    }

                    if (hideSoundEffect != null && !laserPaused)
                    {
                        StartCoroutine(PlayHideSFXAndPauseLaser(hit.point));
                    }

                    hit.collider.gameObject.SetActive(false);
                }

                if (((1 << hit.collider.gameObject.layer) & stopLayers) != 0)
                {
                    break;
                }

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

        // Spawn particle at the final point and set it as child of laser object
        if (laserEndEffect != null && hitPoints.Count > 0)
        {
            Vector2 finalPoint = hitPoints[hitPoints.Count - 1];
            GameObject effect = Instantiate(laserEndEffect, finalPoint, Quaternion.identity, transform); // Make laser object the parent

            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                float duration = ps.main.duration + ps.main.startLifetime.constantMax;
                Destroy(effect, duration);
            }
            else
            {
                Destroy(effect, 2f); // fallback if no ParticleSystem
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




    private IEnumerator PlayHideSFXAndPauseLaser(Vector2 position)
    {
        laserPaused = true;

        if (laserLoopSource != null && laserLoopSource.isPlaying)
        {
            laserLoopSource.Pause();
        }

        GameObject tempAudio = new GameObject("TempHideSFX");
        tempAudio.transform.position = position;
        AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
        tempSource.clip = hideSoundEffect;
        tempSource.Play();

        yield return new WaitForSeconds(hideSoundEffect.length);

        Destroy(tempAudio);

        if (laserLoopSource != null && !laserLoopSource.isPlaying)
        {
            laserLoopSource.UnPause();
        }

        laserPaused = false;
    }

    private IEnumerator PitchLerpLoop()
    {
        while (true)
        {
            float timer = 0f;
            float startPitch = minPitch;
            float endPitch = maxPitch;

            while (timer < pitchLerpDuration)
            {
                timer += Time.deltaTime;
                if (laserLoopSource != null)
                {
                    float t = timer / pitchLerpDuration;
                    laserLoopSource.pitch = Mathf.Lerp(startPitch, endPitch, t);
                }
                yield return null;
            }

            timer = 0f;
            startPitch = maxPitch;
            endPitch = minPitch;

            while (timer < pitchLerpDuration)
            {
                timer += Time.deltaTime;
                if (laserLoopSource != null)
                {
                    float t = timer / pitchLerpDuration;
                    laserLoopSource.pitch = Mathf.Lerp(startPitch, endPitch, t);
                }
                yield return null;
            }
        }
    }

    public List<Transform> GetHitObjects()
    {
        return new List<Transform>(hitObjects);
    }
}
