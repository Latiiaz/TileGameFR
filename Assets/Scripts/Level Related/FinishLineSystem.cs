using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineSystem : MonoBehaviour
{
    public GameObject[] PressurePlates; // Assign pressure plates in the inspector
    [SerializeField] private bool isVictoryTriggered = false; // Ensure victory sequence only runs once
    public LevelManager levelManager;

    [SerializeField] private float requiredWeight = 100f; // Total weight required to trigger victory
    private float _victoryDelay = 3f;
    [SerializeField] private float requiredTime = 1f;

    [SerializeField] private AudioClip VictorySound;
    private AudioSource audioSource;

    [SerializeField] private string nextLevel;

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

        //Debug.Log($"Total Weight on Finish Line Plates: {totalWeight}");

        if (totalWeight >= requiredWeight && !isVictoryTriggered)
        {
            isVictoryTriggered = true; // Ensure this block runs only once
            NextLevelSequence();
        }
    }

    void VictorySequence()
    {
        //Add delay where the player cant move, get from the movement script and turn canMove to false for all game objects
        StartCoroutine(VictoryCoroutine(_victoryDelay));
    }
    void NextLevelSequence()
    {
        StartCoroutine(NextLevelCoroutine(_victoryDelay));
    }
    private IEnumerator NextLevelCoroutine(float seconds)
    {
        audioSource.clip = VictorySound;
        audioSource.Play();
        yield return new WaitForSeconds(seconds);
        levelManager.LoadNextScene();
    }
    private IEnumerator VictoryCoroutine(float seconds)
    {
        audioSource.clip = VictorySound;
        audioSource.Play();
        yield return new WaitForSeconds(seconds);
        levelManager.LoadVictoryScene();
    }

    private IEnumerator DelayCheck(float seconds)
    {
        audioSource.clip = VictorySound;
        audioSource.Play();
        yield return new WaitForSeconds(seconds);
        levelManager.LoadVictoryScene();
    }
}
