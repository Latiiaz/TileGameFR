using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider SFXVolumeSlider;
    public Slider BGMVolumeSlider;

    void Start()
    {
        // Set initial values for the sliders based on the current settings
        masterVolumeSlider.value = SoundManager.Instance.masterVolume;
        SFXVolumeSlider.value = SoundManager.Instance.sfxVolume;
        BGMVolumeSlider.value = SoundManager.Instance.bgmVolume;

        // Add listeners to the sliders to update values in SoundManager
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        SFXVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        BGMVolumeSlider.onValueChanged.AddListener(SetBGMVolume);
    }

    public void SetMasterVolume(float volume)
    {
        SoundManager.Instance.SetMasterVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        SoundManager.Instance.SetSFXVolume(volume);
    }

    public void SetBGMVolume(float volume)
    {
        SoundManager.Instance.SetBGMVolume(volume);
    }
}
