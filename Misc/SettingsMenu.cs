using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class SettingsMenu : MonoBehaviour
{
    public MenuCamera menuCam;
    public GameObject optionsMenuUI;
    public GameObject optionsProfileMenuUI;
    public GameObject optionsGraphicsMenuUI;
    public GameObject optionsControlsMenuUI;
    public GameObject optionsAudioMenuUI;
    private bool firstToggle = true;

    public TMP_Dropdown fsDropdown;
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    public TMP_Dropdown qualityDropdown;

    public bool canClickButton = false;

    public TMP_Dropdown ssDropdown;
    public TMP_InputField usernameInput;
    public GameObject usernameEnterPrompt;

    public Toggle promptToggle;

    public GameObject keyboardBindings;
    public GameObject gamepadBindings;

    // Start is called before the first frame update
    void Start()
    {
        menuCam = GameObject.FindObjectOfType<MenuCamera>();

        StartCoroutine("CanClick");

        optionsMenuUI.SetActive(true);
        optionsGraphicsMenuUI.SetActive(false);
        optionsControlsMenuUI.SetActive(false);
        optionsAudioMenuUI.SetActive(false);

        resolutions = Screen.resolutions;
        //resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();      // The list of resolutions that is shown to the player?
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @" + resolutions[i].refreshRate + "hz";
            options.Add(option);

            if (PlayerPrefs.GetInt("SelectedResolution", -1) == -1)       // First time playing (or never changed resolution??)
            {
                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }
            else        // Set to the player's preference
            {
                currentResolutionIndex = PlayerPrefs.GetInt("SelectedResolution", -1);
            }
        }
        resolutionDropdown.AddOptions(options);
        int selectedResolution = PlayerPrefs.GetInt("SelectedResolution", -1);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        int selectedFullscreenMode = PlayerPrefs.GetInt("SelectedFullscreenMode", 0);
        fsDropdown.value = selectedFullscreenMode;  //fullscreenDropdown value - 0 (Fullscreen Window), 1 (Exclusive Fullscreen), 2 (Maximized Window [not supported on Windows]), 3 (Windowed)
        SetFullscreen();

        int selectedQualityPref = PlayerPrefs.GetInt("SelectedQuality", 3);
        qualityDropdown.value = selectedQualityPref;
        SetQuality(selectedQualityPref);

        int selectedServer = PlayerPrefs.GetInt("SelectedServer", 0);
        ssDropdown.value = selectedServer;
        ServerSelection();

        if (PlayerPrefs.GetInt("PromptsToggle", 1) == 1)
        {
            promptToggle.isOn = true;
        }
        else
        {
            promptToggle.isOn = false;
        }
    }

    public IEnumerator CanClick()
    {
        yield return new WaitForSeconds(3f);
        canClickButton = true;
        ssDropdown.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void LoadOptions()
    {
        Debug.Log("Loading the options...");
        if (firstToggle)
        {
            LoadProfile();
            firstToggle = false;
        }
        //audioSource.Play();

    } // End of LoadOptions()

    public void OptionsBack()
    {

    } // End of OptionsBack()

    public void LoadProfile()
    {
        if (canClickButton)
        {
            optionsProfileMenuUI.SetActive(true);
            optionsGraphicsMenuUI.SetActive(false);
            optionsControlsMenuUI.SetActive(false);
            optionsAudioMenuUI.SetActive(false);
        }
    }

    public void ProfileBack()
    {
        if (canClickButton)
        {
            optionsProfileMenuUI.SetActive(false);
            optionsGraphicsMenuUI.SetActive(false);
            optionsControlsMenuUI.SetActive(false);
            optionsAudioMenuUI.SetActive(false);
        }
    }

    public void LoadGraphics()
    {
        if (canClickButton)
        {
            optionsProfileMenuUI.SetActive(false);
            optionsGraphicsMenuUI.SetActive(true);
            optionsControlsMenuUI.SetActive(false);
            optionsAudioMenuUI.SetActive(false);
            //audioSource.Play();
        }
    } // End of Graphics()

    public void LoadControls()
    {
        if (canClickButton)
        {
            optionsProfileMenuUI.SetActive(false);
            optionsGraphicsMenuUI.SetActive(false);
            optionsControlsMenuUI.SetActive(true);
            optionsAudioMenuUI.SetActive(false);
            //audioSource.Play();
            keyboardBindings.SetActive(Gamepad.current == null);
            gamepadBindings.SetActive(Gamepad.current != null);

        }
    } // End of Controls()

    public void LoadAudio()
    {
        if (canClickButton)
        {
            optionsProfileMenuUI.SetActive(false);
            optionsGraphicsMenuUI.SetActive(false);
            optionsControlsMenuUI.SetActive(false);
            optionsAudioMenuUI.SetActive(true);
            //audioSource.Play();
        }
    } // End of Audio()

    public void GoToLobby()
    {
        if (canClickButton)
        {
            menuCam.GoToElevator();
        }
    }

    public void LoadSettings()
    {
        if (canClickButton)
        {
            menuCam.GoToSettings();
        }
    }

    public void BackToTitle()
    {
        if (usernameInput.text.Length == 0)
        {
            usernameEnterPrompt.SetActive(true);
        }
        else if (canClickButton)
        {
            usernameEnterPrompt.SetActive(false);
            menuCam.GoToTitle();
        }
    }
    public void QuitGame()
    {
        if (canClickButton)
        {
            //audioSource.Play();
            Debug.Log("Quitting the game...");
            Application.Quit();
        }

    } // End of QuitGame()

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("SelectedQuality", qualityIndex);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("SelectedResolution", resolutionIndex);
    }

    public void SetFullscreen()
    {
        if (fsDropdown.options[fsDropdown.value].text == "Fullscreen Window")
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Debug.Log("Fullscreen Window");
            Screen.fullScreen = true;
            PlayerPrefs.SetInt("SelectedFullscreenMode", 0);
        }
        else if (fsDropdown.options[fsDropdown.value].text == "Exclusive Fullscreen")
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            Debug.Log("Exclusive Fullscreen");
            Screen.fullScreen = true;
            PlayerPrefs.SetInt("SelectedFullscreenMode", 1);
        }
        else if (fsDropdown.options[fsDropdown.value].text == "Maximized Window")
        {
            Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
            Debug.Log("Maximized Window");
            //Screen.fullScreen = false;
            PlayerPrefs.SetInt("SelectedFullscreenMode", 2);
        }
        else if (fsDropdown.options[fsDropdown.value].text == "Windowed")
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Debug.Log("Windowed");
            //Screen.fullScreen = false;
            PlayerPrefs.SetInt("SelectedFullscreenMode", 3);
        }
    }

    public void ServerSelection()
    {
        switch (ssDropdown.options[ssDropdown.value].text)
        {
            case ("North America East"):
                Debug.Log("USE");
                PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "us";
                PlayerPrefs.SetInt("SelectedServer", 0);
                break;

            case ("North America West"):
                Debug.Log("USW");
                PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "usw";
                PlayerPrefs.SetInt("SelectedServer", 1);
                break;

            case ("Europe"):
                Debug.Log("EU");
                PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "eu";
                PlayerPrefs.SetInt("SelectedServer", 2);
                break;

            case ("Asia"):
                Debug.Log("ASIA");
                PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "asia";
                PlayerPrefs.SetInt("SelectedServer", 3);
                break;

            default:
                Debug.Log("DEFAULT US");
                PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "us";
                PlayerPrefs.SetInt("SelectedServer", 0);
                break;
        }

    }

    public void SetPromptToggle()
    {
        if (promptToggle.isOn)
        {
            PlayerPrefs.SetInt("PromptsToggle", 1);
        }
        else
        {
            PlayerPrefs.SetInt("PromptsToggle", 0);
        }
    }
}