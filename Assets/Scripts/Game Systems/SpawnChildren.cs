using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChildren : MonoBehaviour
{
    public float fadeDuration = 1.5f; // Time it takes to reveal all children
    public GameObject particlePrefab; // Particle effect prefab

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
            sr.enabled = true; // Instantly appear

            // Spawn particle effect
            if (particlePrefab != null)
            {
                GameObject particleEffect = Instantiate(particlePrefab, sr.transform.position, Quaternion.identity);
                ParticleSystem ps = particleEffect.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    Destroy(particleEffect, ps.main.duration); // Destroy when particle effect ends
                }
                else
                {
                    Destroy(particleEffect, 2f); // Fallback in case no ParticleSystem is attached
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
