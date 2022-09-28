using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // -- Declared Variables -- //
    //public static bool gameManagerExists;
    public bool dialogueIsRunning;
    public Flowchart introOfficeFlowchart;
    public static bool firstLoadOfOffice = true;
    public InventoryUI inventoryUI;
    // -- -- //

    private void OnLevelWasLoaded(int level)
    {
        if (level > 1)
        {
            Debug.Log("GameMananger update UI");
            inventoryUI.UpdateUI();
        }
    }

    public void UpdateDialogueVariable()
    {
        if (dialogueIsRunning == true)
        {
            dialogueIsRunning = false;
            Debug.Log("Dialogue is off");
        }
        else if (dialogueIsRunning == false)
        {
            dialogueIsRunning = true;
            Debug.Log("Dialogue is on");
        }
    }

    public void UpdateLoadOfficeVariable()
    {
        if (firstLoadOfOffice == true)
        {
            introOfficeFlowchart.ExecuteBlock("Into Inner Monologue");
            firstLoadOfOffice = false;
        }
        else
        {
            // Player has been to the Office scene already
        }

    }

    public void ResetOfficeVariable()
    {
        firstLoadOfOffice = true;
        Debug.Log(firstLoadOfOffice);
    }
}
