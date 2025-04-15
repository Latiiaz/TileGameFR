using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Unused Currently")]
    public Slider masterVolumeSlider;
    public Slider SFXVolumeSlider;
    public Slider BGMVolumeSlider;

    [Header("Selection")]
    [SerializeField] GameObject MainMenuBox;
    [SerializeField] private bool _mainMenuSelected;

    [SerializeField] GameObject ResumeBox;
    [SerializeField] private bool _resumeSelected;

    private LevelManager _levelManager;

    void Start()
    {
        // Get LevelManager instance
        _levelManager = FindObjectOfType<LevelManager>();
        // Set initial values for the sliders based on the current settings
        masterVolumeSlider.value = SoundManager.Instance.masterVolume;
        SFXVolumeSlider.value = SoundManager.Instance.sfxVolume;
        BGMVolumeSlider.value = SoundManager.Instance.bgmVolume;

        // Add listeners to the sliders to update values in SoundManager
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        SFXVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        BGMVolumeSlider.onValueChanged.AddListener(SetBGMVolume);

        MainMenuBox.SetActive(false);
        ResumeBox.SetActive(false);
    }

    private void Update()
    {
        CheckStatus();
        EnterSelected();
    }

    void CheckStatus()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _mainMenuSelected = true;
            _resumeSelected = false;
            MainMenuBox.SetActive(true);
            ResumeBox.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            _resumeSelected = true;
            _mainMenuSelected = false;
            MainMenuBox.SetActive(false);
            ResumeBox.SetActive(true);
        }
    }

    void EnterSelected()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_mainMenuSelected)
            {
                Debug.Log("Main Menu Button Selected");
                _levelManager.LoadMainMenu(); // Load Main Menu
            }

            if (_resumeSelected)
            {
                Debug.Log("Resume Button Selected");
                // You can add resume logic here if needed
            }
        }
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
