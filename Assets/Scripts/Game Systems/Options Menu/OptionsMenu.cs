using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{

    [Header("Menu Root")]
    [SerializeField] private GameObject MenuRoot;
    [SerializeField] private bool _isActivated = false;

    [Header("Selection Boxes")]
    [SerializeField] private GameObject MainMenuBox;
    [SerializeField] private GameObject ResumeBox;

    [Header("Selection Text Labels")]
    [SerializeField] private TextMeshProUGUI MainMenuText;
    [SerializeField] private TextMeshProUGUI ResumeText;

    [SerializeField] private bool _mainMenuSelected;
    [SerializeField] private bool _resumeSelected;

    private LevelManager _levelManager;

    private float escapeCooldown = 0.2f;
    private float lastEscapeTime = -Mathf.Infinity;

    void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();

        MainMenuBox.SetActive(false);
        ResumeBox.SetActive(true);
        MainMenuText.gameObject.SetActive(false);
        ResumeText.gameObject.SetActive(true);
        MenuRoot.SetActive(false);

        // Set default selection
        _resumeSelected = true;
        _mainMenuSelected = false;
    }

    void Update()
    {
        CheckStatus();
        EnterSelected();
    }

    void ToggleMenu()
    {
        if (_isActivated)
        {
            MenuRoot.SetActive(true);
            _isActivated = false;

            // Default to resume selection on open
            SelectResume();
        }
        else
        {
            MenuRoot.SetActive(false);
            _isActivated = true;
        }
    }

    void CheckStatus()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SelectMainMenu();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SelectResume();
        }
    }

    void SelectMainMenu()
    {
        _mainMenuSelected = true;
        _resumeSelected = false;
        UpdateSelectionVisuals();
    }

    void SelectResume()
    {
        _resumeSelected = true;
        _mainMenuSelected = false;
        UpdateSelectionVisuals();
    }

    void EnterSelected()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Spacebar pressed");

            if (_mainMenuSelected)
            {
                Debug.Log("Main Menu Button Selected");
                Time.timeScale = 1f;

                if (_levelManager != null)
                    _levelManager.LoadMainMenu();
                else
                    Debug.LogWarning("LevelManager is null!");
            }
            else if (_resumeSelected)
            {
                Debug.Log("Resume Button Selected");
                ToggleMenu();
            }
            else
            {
                Debug.Log("Nothing is selected!");
            }
        }
    }

    void UpdateSelectionVisuals()
    {
        MainMenuBox.SetActive(_mainMenuSelected);
        ResumeBox.SetActive(_resumeSelected);

        MainMenuText.gameObject.SetActive(_mainMenuSelected);
        ResumeText.gameObject.SetActive(_resumeSelected);
    }

    public void SetBGMVolume(float volume)
    {
        SoundManager.Instance.SetBGMVolume(volume);
    }
}
