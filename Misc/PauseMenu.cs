using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    // -- Declared Variables -- //
    public GameObject scoreMenuUI;
    public GameObject iDBookMenuUI;
    public static bool iDBookIsOpen;
    public static bool gameIsPaused = false;
    public GameObject pauseMenuParentObj;
    public GameObject pauseMenuBackground;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject optionsGraphicsMenuUI;
    public GameObject optionsAudioMenuUI;

    public TMP_Dropdown fsDropdown;
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    public TMP_Dropdown graphicDropdown;

    public Animator pauseMenuAnimator;
    private bool firstPauseToggle = true;

    // Start is called before the first frame update
    void Start()
    {
        scoreMenuUI.SetActive(false);
        gameIsPaused = false;
        optionsMenuUI.SetActive(false);
        optionsGraphicsMenuUI.SetActive(false);
        optionsAudioMenuUI.SetActive(false);

        // Graphics Settings Checks/Setup
        resolutions = Screen.resolutions;
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @" + resolutions[i].refreshRate + "hz";
            options.Add(option);

            if (PlayerPrefs.GetInt("SelectedResolution", -1) == -1)       // First time playing (or never changed resolution)
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
        graphicDropdown.value = selectedQualityPref;
        SetQuality(selectedQualityPref);

    } // End of Start()

    public void PauseToggle()
    {
        if (gameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }

    } // End of PauseToggle()

    public void GoTitleMenu()
    {
        SceneManager.LoadScene(0);      // Scene 0 is title menu in build settings

    } // End of GoTitleMenu()

    public void ResetScene()
    {
        SceneManager.LoadScene(1);      // Scene 1 is greenhouse scene in build settings

    } // End of ResetScene()

    public void ToggleIDBook()
    {
        if (iDBookIsOpen)
        {
            iDBookMenuUI.SetActive(false);
            iDBookIsOpen = false;
        }
        else
        {
            iDBookMenuUI.SetActive(true);
            iDBookIsOpen = true;
        }

    } // End of ToggleIDBook()

    public void Pause()
    {
        // Make sure ID book is closed if player tries to pause
        if (iDBookIsOpen)
        {
            ToggleIDBook();
        }
        gameIsPaused = true;
        pauseMenuBackground.SetActive(true);
        pauseMenuAnimator.SetBool("isPaused", true);

    } // End of Pause()

    public void Resume()
    {
        gameIsPaused = false;
        pauseMenuBackground.SetActive(false);
        pauseMenuAnimator.SetBool("isPaused", false);

    } // End of Resume()

    public void LoadOptions()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
        // Show Audio fist if it's the first load
        if (firstPauseToggle)
        {
            LoadAudio();
            firstPauseToggle = false;
        }

    } // End of LoadOptions()

    public void OptionsBack()
    {
        pauseMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);

    } // End of OptionsBack()


    public void LoadGraphics()
    {
        optionsGraphicsMenuUI.SetActive(true);
        optionsAudioMenuUI.SetActive(false);

    } // End of LoadGraphics()

    public void LoadAudio()
    {
        optionsGraphicsMenuUI.SetActive(false);
        optionsAudioMenuUI.SetActive(true);

    } // End of LoadAudio()

    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();

    } // End of QuitGame()

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("SelectedQuality", qualityIndex);
    } // End of SetQuality(int qualityIndex)

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("SelectedResolution", resolutionIndex);
    } // End of SetResolution(int resolutionIndex)

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
            PlayerPrefs.SetInt("SelectedFullscreenMode", 2);
        }
        else if (fsDropdown.options[fsDropdown.value].text == "Windowed")
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Debug.Log("Windowed");
            PlayerPrefs.SetInt("SelectedFullscreenMode", 3);
        }
    } // End of SetFullscreen()

}
