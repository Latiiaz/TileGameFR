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
        float totalWeight = 0f;

        foreach (GameObject pressurePlate in PressurePlates)
        {
            if (pressurePlate.CompareTag("PressurePlate"))
            {
                PressurePlateSystem plateSystem = pressurePlate.GetComponent<PressurePlateSystem>();
                if (plateSystem != null)
                {
                    totalWeight += plateSystem.GetTotalWeight();
                }
            }
        }

        Debug.Log($"Total Weight on Finish Line Plates: {totalWeight}");

        if (totalWeight >= requiredWeight && !isVictoryTriggered)
        {
            isVictoryTriggered = true; // Ensure this block runs only once
            VictorySequence();
        }
    }

    void VictorySequence()
    {
        StartCoroutine(VictoryCoroutine(_victoryDelay));
    }

    private IEnumerator VictoryCoroutine(float seconds)
    {
        audioSource.clip = VictorySound;
        audioSource.Play();
        yield return new WaitForSeconds(seconds);
        levelManager.LoadVictoryScene();
    }
}
