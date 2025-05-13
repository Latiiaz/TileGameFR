using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PPManager : MonoBehaviour
{
    public static PPManager Instance { get; private set; }

    [SerializeField] private PostProcessVolume volume;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PulseBloom(float targetIntensity, float totalDuration)
    {
        StopAllCoroutines(); // optional: prevent overlaps
        StartCoroutine(PulseBloomCoroutine(targetIntensity, totalDuration));
    }

    private IEnumerator PulseBloomCoroutine(float targetIntensity, float totalDuration)
    {
        if (!volume.profile.TryGetSettings(out Bloom bloom))
            yield break;

        float originalIntensity = bloom.intensity.value;
        float halfDuration = totalDuration / 2f;
        float elapsed = 0f;

        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            float value = Mathf.Lerp(originalIntensity, targetIntensity, t);
            bloom.intensity.Override(value);
            yield return null;
        }

        elapsed = 0f;

        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            float value = Mathf.Lerp(targetIntensity, originalIntensity, t);
            bloom.intensity.Override(value);
            yield return null;
        }

        bloom.intensity.Override(originalIntensity);
    }

}
