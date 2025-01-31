using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public List<AudioSource> bgmAudioSources = new List<AudioSource>();
    public AudioSource sfxAudioSource; // Dedicated source for playing SFX
    public List<AudioSource> sfxAudioSources = new List<AudioSource>();

    [Header("Volume Levels")]
    [Range(0f, 1f)]
    public float masterVolume = 1f;
    [Range(0f, 1f)]
    public float sfxVolume = 1f;
    [Range(0f, 1f)]
    public float bgmVolume = 1f;

    [Header("Audio Clips")]
    public List<AudioClip> sfxClips; // List of all SFX AudioClips
    public List<AudioClip> bgmClips; // List of all BGM AudioClips

    private Dictionary<string, AudioClip> sfxClipDictionary; // Map for SFX Clips
    private Dictionary<string, AudioClip> bgmClipDictionary; // Map for BGM Clips

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        // Initialize dictionaries
        sfxClipDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in sfxClips)
        {
            sfxClipDictionary[clip.name] = clip;
        }

        bgmClipDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in bgmClips)
        {
            bgmClipDictionary[clip.name] = clip;
        }
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        AudioListener.volume = masterVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;

        foreach (AudioSource sfx in sfxAudioSources)
        {
            sfx.volume = sfxVolume;
        }
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;

        foreach (AudioSource bgm in bgmAudioSources)
        {
            bgm.volume = bgmVolume;
        }
    }

    public void PlaySFXSound(string soundName)
    {
        if (sfxClipDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            sfxAudioSource.PlayOneShot(clip, sfxVolume); // Play the clip at the current SFX volume
        }
        else
        {
            Debug.LogWarning($"SFX sound '{soundName}' not found!");
        }
    }

    public void PlayBGMSound(string soundName, bool loop = true)
    {
        if (bgmClipDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            AudioSource bgmSource = bgmAudioSources.Count > 0 ? bgmAudioSources[0] : null; // Use the first BGM source

            if (bgmSource != null)
            {
                bgmSource.clip = clip;
                bgmSource.volume = bgmVolume;
                bgmSource.loop = loop;
                bgmSource.Play();
            }
        }
        else
        {
            Debug.LogWarning($"BGM sound '{soundName}' not found!");
        }
    }
}
