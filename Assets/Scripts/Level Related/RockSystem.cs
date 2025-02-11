using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSystem : MonoBehaviour, ITractor
{
    [SerializeField] private AudioClip rockBreakSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
    }

    //public void InteractE()
    //{
    //    Debug.Log("E key works");

    //    if (rockBreakSound != null && audioSource != null)
    //    {
    //        audioSource.clip = rockBreakSound;
    //        audioSource.Play();
    //        StartCoroutine(DestroyAfterSound()); 
    //    }
    //}

    public void InteractF()
    {
        Debug.Log("Awing baweh");

        if (rockBreakSound != null && audioSource != null)
        {
            audioSource.clip = rockBreakSound;
            audioSource.Play();
            StartCoroutine(DestroyAfterSound());
        }
    }

    private IEnumerator DestroyAfterSound()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(gameObject);
    }
}

