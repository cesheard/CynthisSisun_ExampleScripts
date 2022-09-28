using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // -- Declared Vairables -- //
    public GameObject mainMenuUI;
    public GameObject optionsMenuUI;
    public AudioMixer audioMixer;

    // Resolution
    public Dropdown resolutionDropdown;
    Resolution[] resolutions;

    public InventoryList inventoryList;
    public GameManager gameManager;
    // -- -- //

    public void Start()
    {
        // Make sure the right things are active at the start of the game.
        mainMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);

        // Find out what resolutions are avaliable
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    } // End of Start()

    public void NewGame()
    {
        Debug.Log("Loading a new save...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        inventoryList.items.Clear();
        gameManager.ResetOfficeVariable();

    } // End of NewGame()

    public void LoadOptions()
    {
        Debug.Log("Loading options...");
        mainMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);

    } // End of LoadOptions()

    public void OptionsBack()
    {
        optionsMenuUI.SetActive(false);
        mainMenuUI.SetActive(true);

    } // End of OptionsBack

    public void QuitGame()
    {
        Debug.Log("Quiting the game...");
        Application.Quit();

    } // End of QuitGame()

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);

    } // End of SetVolume(float volume)

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

    } // End of SetFullscreen(bool isFullscreen)

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    } // End of SetResolution(int resolutionIndex)

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);

    } // End of SetQuality(int qualityIndex)

}
