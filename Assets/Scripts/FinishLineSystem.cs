using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineSystem : MonoBehaviour
{
    public GameObject[] PressurePlates;
    [SerializeField] private bool isVictoryTriggered = false; // Ensure victory sequence only runs once

    public LevelManager levelManager;
    private float _victoryDelay = 3f;

    [SerializeField] private AudioClip VictorySound;
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

    void Update()
    {
        if (!isVictoryTriggered)
        {
            CheckPressurePlates();
        }
    }

    public void CheckPressurePlates()
    {
        bool allPlatesActive = true;

        for (int i = 0; i < PressurePlates.Length; i++)
        {
            GameObject pressureplate = PressurePlates[i];
            if (pressureplate.CompareTag("PressurePlate"))
            {
                PressurePlateSystem plateSystem = pressureplate.GetComponent<PressurePlateSystem>();
                if (plateSystem != null)
                {
                    bool plateOutput = plateSystem.GetOutputStatus();

                    if (!plateOutput)
                    {
                        allPlatesActive = false;
                    }
                }
            }
            else
            {
                allPlatesActive = false;
            }
        }

        if (allPlatesActive && !isVictoryTriggered)
        {
            isVictoryTriggered = true; // Ensure this block runs only once
            VictorySequence();
        }
    }

    void VictorySequence()
    {
        StartCoroutine(WaitForSecondsCoroutine(_victoryDelay));
    }

    private IEnumerator WaitForSecondsCoroutine(float seconds)
    {
        audioSource.clip = VictorySound;
        audioSource.Play();
        yield return new WaitForSeconds(seconds);
        levelManager.LoadVictoryScene();
    }
}
