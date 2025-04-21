using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChildren : MonoBehaviour
{
    public float fadeDuration = 1.5f; // Time it takes to reveal all children
    public GameObject particlePrefab; // Particle effect prefab
    public AudioClip spawnSound; // Sound to play when a sprite is revealed
    public float soundVolume = 1f; // Volume of the sound
    public float spawnSoundChance = 0.4f; // 40% chance to play the sound

    void Start()
    {
        StartCoroutine(RandomRevealAllChildren());
    }

    IEnumerator RandomRevealAllChildren()
    {
        List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
        GetAllChildSprites(transform, spriteRenderers);

        // Hide all children initially
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.enabled = false;
        }

        // Shuffle list for randomness
        yield return new WaitForSeconds(1f);

        System.Random rng = new System.Random();
        for (int i = spriteRenderers.Count - 1; i > 0; i--)
        {
            int randomIndex = rng.Next(i + 1);
            (spriteRenderers[i], spriteRenderers[randomIndex]) = (spriteRenderers[randomIndex], spriteRenderers[i]);
        }

        // Calculate the delay between each reveal
        float delayBetweenReveals = fadeDuration / spriteRenderers.Count;

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            yield return new WaitForSeconds(delayBetweenReveals / 2);
            sr.enabled = true;

            // Random chance to play spawn sound
            if (spawnSound != null && Random.Range(0f, 1f) < spawnSoundChance)
            {
                AudioSource.PlayClipAtPoint(spawnSound, sr.transform.position, soundVolume);
            }

            // Spawn particle effect
            if (particlePrefab != null)
            {
                GameObject particleEffect = Instantiate(particlePrefab, sr.transform.position, Quaternion.identity);
                ParticleSystem ps = particleEffect.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    Destroy(particleEffect, ps.main.duration);
                }
                else
                {
                    Destroy(particleEffect, 3f);
                }
            }
        }
    }

    void GetAllChildSprites(Transform parent, List<SpriteRenderer> spriteList)
    {
        foreach (Transform child in parent)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                spriteList.Add(sr);
            }
            GetAllChildSprites(child, spriteList); // Recursively get deeper children
        }
    }
}
