using System.Collections;
using UnityEngine;

public class FinishLineSystem : MonoBehaviour
{
    public GameObject[] PressurePlates; // Assign pressure plates in the inspector
    [SerializeField] private bool isVictoryTriggered = false;
    public LevelManager levelManager;

    [SerializeField] private float requiredWeight = 100f;
    [SerializeField] private float requiredTime = 1f;

    [SerializeField] private AudioClip VictorySound;
    private AudioSource audioSource;

    [SerializeField] private string nextLevel;
    [SerializeField] private GameObject PlayerParticle;
    [SerializeField] private GameObject TractorParticle;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (!isVictoryTriggered)
        {
            CheckPressurePlates();
        }
    }

    public void CheckPressurePlates()
    {
        float totalWeight = 0f;

        foreach (GameObject pressurePlate in PressurePlates)
        {
            if (pressurePlate.CompareTag("PressurePlate"))
            {
                FinalPPSystem plateSystem = pressurePlate.GetComponent<FinalPPSystem>();
                if (plateSystem != null)
                {
                    totalWeight += plateSystem.GetTotalWeight();
                }
            }
        }

        if (totalWeight >= requiredWeight && !isVictoryTriggered)
        {
            isVictoryTriggered = true;
            FindObjectOfType<GameManager>().DisableInputTemporarily(3f);
            StartCoroutine(NextLevelCoroutine());
        }
    }

    private IEnumerator NextLevelCoroutine()
    {
        //audioSource.clip = VictorySound;
        //audioSource.Play();
        yield return StartCoroutine(ShrinkWithBounce());

        yield return new WaitForSeconds(0.5f);
        levelManager.LoadNextScene();
    }

    private IEnumerator ShrinkWithBounce()
    {
        GameObject _player = GameObject.FindWithTag("Player");
        GameObject _tractor = GameObject.FindWithTag("Tractor");

        float duration = 1.2f;
        float timer = 0f;
        bool bloomTriggered = false;

        Vector3 playerStart = _player != null ? _player.transform.localScale : Vector3.zero;
        Vector3 tractorStart = _tractor != null ? _tractor.transform.localScale : Vector3.zero;

        while (timer < duration)
        {
            float t = timer / duration;

            // Trigger bloom once when halfway through
            if (!bloomTriggered && t > 0.2f)
            {
                bloomTriggered = true;
                if (PPManager.Instance != null)
                {
                    PPManager.Instance.PulseBloom(25f, duration * 0.4f); // You can adjust duration/intensity here
                }
            }

            // Bounce easing
            float bounce = Mathf.Sin(t * Mathf.PI * 3f) * (1f - t) * 0.2f;
            float smoothT = Mathf.SmoothStep(1f, 0f, t);
            float scaleValue = smoothT + bounce;

            if (_player != null)
                _player.transform.localScale = playerStart * Mathf.Max(0f, scaleValue);

            if (_tractor != null)
                _tractor.transform.localScale = tractorStart * Mathf.Max(0f, scaleValue);

            timer += Time.deltaTime;
            yield return null;
        }

        // Finalize scaling
        if (_player != null) _player.transform.localScale = Vector3.zero;
        if (_tractor != null) _tractor.transform.localScale = Vector3.zero;

        // Play vanish particles at the positions of both player and tractor
        audioSource.clip = VictorySound;
        audioSource.Play();
        if (_player != null)
            {
                Instantiate(PlayerParticle, _player.transform.position, Quaternion.identity);
            }

            if (_tractor != null)
            {
                Instantiate(TractorParticle, _tractor.transform.position, Quaternion.identity);
            }
        

        // Trigger camera shake
        CameraManager camManager = FindObjectOfType<CameraManager>();
        if (camManager != null)
        {
            StartCoroutine(camManager.ShakeCamera(0f, 1.2f)); // delay = 0s, strength = 1.2f
        }
        else
        {
            Debug.LogWarning("CameraManager not found for shake.");
        }
    }

}