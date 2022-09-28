using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using Fungus;

// Handles the main, pause, case file, map, inerrogation menus
public class UIManager : MonoBehaviour
{
    // -- Declared Variables -- //
    public GameObject mainMenuUI;
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public AudioMixer audioMixer;

    // Case file stuff
    public static bool caseFileOpen = false;
    public GameObject baseCaseFileUI;
    //public GameObject cluesMenuUI;
    public GameObject inventoryMenuUI;
    public GameObject dialogueMenuUI;
    public Text itemTextComp;  // Refenrence to UI item text component
    InventoryList inventoryList;
    public AudioClip pageTurning;

    // Map stuff
    public GameObject baseMapUI;

    // Interrogation
    public bool isBeingInterrogated;
    public GameObject InterrogationUI;
    //public GameObject BaseCaseFileUI;

    // Resolution
    public Dropdown resolutionDropdown;
    Resolution[] resolutions;

    public GameManager gameManager;     // Reference to the GameManager script
    public Flowchart flowchart;     // Reference to the Fungus flowchart in the scene
    // -- -- //

    void Start()
    {
        // Make sure the right things are active at the start of the game
        mainMenuUI.SetActive(true);
        gameIsPaused = false;
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        Time.timeScale = 1;

        baseCaseFileUI.SetActive(false);
        baseMapUI.SetActive(false);
        isBeingInterrogated = false;
        InterrogationUI.SetActive(false);

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

    // Update is called once per frame
    void Update()
    {
        // If esc is pressed, pause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    } // Emd of Update()

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
        flowchart.SetBooleanVariable("isBusy", true);
        Debug.Log(flowchart.GetBooleanVariable("isBusy"));

    } // End of Pause()

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
        flowchart.SetBooleanVariable("isBusy", false);
        Debug.Log(flowchart.GetBooleanVariable("isBusy"));

    } // End of Resume()

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        Debug.Log("Loading the Main Menu...");
        SceneManager.LoadScene(0);

    } // End of LoadMainMenu()

    public void LoadOptions()
    {
        Debug.Log("Loading the options...");
        if (gameIsPaused)
        {
            pauseMenuUI.SetActive(false);
        }
        else
        {
            mainMenuUI.SetActive(false);
        }
        optionsMenuUI.SetActive(true);


    } // End of LoadOptions()

    public void OptionsBack()
    {
        optionsMenuUI.SetActive(false);
        if (gameIsPaused)
        {
            pauseMenuUI.SetActive(true);
        }
        else
        {
            mainMenuUI.SetActive(true);
        }

    } // End of OptionsBack()

    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
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

    public void showBaseCaseFileUI()
    {
        if (gameManager.dialogueIsRunning == false)
        {
            baseCaseFileUI.SetActive(true);
            flowchart.SetBooleanVariable("isBusy", true);
            GameObject.Find("CaseFileIconButton").GetComponent<AudioSource>().PlayOneShot(pageTurning);
        }
        else
        {
            Debug.Log("Opening the Case File has been prevented because dialogue is happening");
        }

    } // End showBaseCaseFileUI()

    public void hideCaseFileUI()
    {

        if (isBeingInterrogated == true)
        {
            InterrogationUI.SetActive(true);
            flowchart.ExecuteBlock("Interrogation");
        }
        baseCaseFileUI.SetActive(false);
        GameObject.Find("CaseFileIconButton").GetComponent<AudioSource>().PlayOneShot(pageTurning);
        ClearDescription();
        flowchart.SetBooleanVariable("isBusy", false);

    } // End hideCaseFileUI()

    public void OpenClues()
    {
        baseCaseFileUI.SetActive(true);
        //cluesMenuUI.SetActive(true);
        inventoryMenuUI.SetActive(false);
        dialogueMenuUI.SetActive(false);
        flowchart.SetBooleanVariable("isBusy", true);
        ClearDescription();

    } // End of OpenClues()

    public void OpenInventory()
    {
        baseCaseFileUI.SetActive(true);
        //cluesMenuUI.SetActive(false);
        inventoryMenuUI.SetActive(true);
        dialogueMenuUI.SetActive(false);
        flowchart.SetBooleanVariable("isBusy", true);
        ClearDescription();

    } // End of OpenInventory()

    public void OpenDialogueHistory()
    {
        baseCaseFileUI.SetActive(true);
        //cluesMenuUI.SetActive(false);
        inventoryMenuUI.SetActive(false);
        dialogueMenuUI.SetActive(true);
        flowchart.SetBooleanVariable("isBusy", true);
        ClearDescription();

    } // End of OpenDialogueHistory()

    void ClearDescription()
    {
        // Reset the item description
        itemTextComp.text = "";
    }

    public void ToggleFullscreenMap()
    {
        if (gameManager.dialogueIsRunning == false)
        {
            // Expand the map
            if (baseMapUI.activeInHierarchy == false)
            {
                baseMapUI.SetActive(true);
                flowchart.SetBooleanVariable("isBusy", true);
            }
            // Shrink the map
            else if (baseMapUI.activeInHierarchy == true)
            {
                baseMapUI.SetActive(false);
                flowchart.SetBooleanVariable("isBusy", false);
            }
        }
    } // End of ToggleFullscreenMap()

    public void GoToOffice()
    {
        SceneManager.LoadScene(1);

    } // End of GoToOffice()

    public void GoToOldLadysHouse()
    {
        SceneManager.LoadScene(2);

    } // End of GoToOldLadysHouse()

    public void GoToMainStreet()
    {
        SceneManager.LoadScene(3);

    } // End of GoToMainStreet()

    public void GoToPark()
    {
        SceneManager.LoadScene(4);

    } // End of GoToPark()

    public void GoToLibrary()
    {
        SceneManager.LoadScene(5);

    } // End of GoToLibrary()

    public void GoToBank()
    {
        SceneManager.LoadScene(6);

    } // End of GoToBank()

    public void GoToNyanCafe()
    {
        SceneManager.LoadScene(7);

    } // End of GoToNyanCafe()

    public void GoToCupcakeShop()
    {
        SceneManager.LoadScene(8);

    } // End of GoToCupcakeShop()

    public void GoToBeContinued()
    {
        SceneManager.LoadScene(9);
    }

    public void Interrogate()
    {
        if (isBeingInterrogated == false)
        {
            // Show the Interrogation UI
            InterrogationUI.SetActive(true);
            isBeingInterrogated = true;
        }
        else if (isBeingInterrogated == true)
        {
            // Hide the Interrogation UI
            InterrogationUI.SetActive(false);
            isBeingInterrogated = false;
        }
    }

    // Gets calls when someone is asked "What's this?"
    public void ShowItem()
    {
        baseCaseFileUI.SetActive(true);
        GameObject.Find("CaseFileIconButton").GetComponent<AudioSource>().PlayOneShot(pageTurning);
    }
}
