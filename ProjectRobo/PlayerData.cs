using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    // -- CYNTHIA'S CODE AHEAD, BEWARE -- //
    public float[] position;
    public bool[] puzzleStatus;
    public bool[] doorStatus;
    public float[,] firstFloorSleepingBays;
    public float[,] secondFloorSleepingBays;

    public PlayerData(PlayerMovement player, LevelManager levelManager)
    {
        // The statuses of the doors when the player save
        int numberOfDoors = levelManager.DoorList.Count;
        doorStatus = new bool[numberOfDoors];
        for (int i = 0; i < numberOfDoors; i++)
        {
            doorStatus[i] = levelManager.DoorList[i].isOpen;
        }

        // The statuses of the puzzles
        int numberOfPuzzles = levelManager.PuzzleUIScript.puzzles.Count;
        puzzleStatus = new bool[numberOfPuzzles];
        for (int i = 0; i < numberOfPuzzles; i++)
        {
            puzzleStatus[i] = levelManager.PuzzleUIScript.puzzles[i].isComplete;
        }


        // Find number of sleeping bays and their positions
        int numberOfFirstFlrBays = levelManager.firstFlrSleepingBays.Count;
        int numberOfSecondFlrBays = levelManager.secondFlrSleepingBays.Count;
        firstFloorSleepingBays = new float[numberOfFirstFlrBays, 3];
        for (int i = 0; i < numberOfFirstFlrBays; i++)
        {
            firstFloorSleepingBays[i, 0] = levelManager.firstFlrSleepingBays[i].transform.position.x;
            firstFloorSleepingBays[i, 1] = levelManager.firstFlrSleepingBays[i].transform.position.y;
            firstFloorSleepingBays[i, 2] = levelManager.firstFlrSleepingBays[i].transform.position.z;
        }
        secondFloorSleepingBays = new float[numberOfSecondFlrBays, 3];
        for (int i = 0; i < numberOfSecondFlrBays; i++)
        {
            secondFloorSleepingBays[i, 0] = levelManager.secondFlrSleepingBays[i].transform.position.x;
            secondFloorSleepingBays[i, 1] = levelManager.secondFlrSleepingBays[i].transform.position.y;
            secondFloorSleepingBays[i, 2] = levelManager.secondFlrSleepingBays[i].transform.position.z;
        }


        // Check what floor we are on
        if (player.transform.position.y < 0)
        {
            // First floor
            float shortestHypoTemp = 99999999999.9f;    // Supposed to be a random ridiculously high number
            int closestBayTemp = -1;
            for (int i = 0; i < firstFloorSleepingBays.GetLength(0); i++)
            {
                // Find the hypotenuse (distance between the two points)
                float differenceX = player.transform.position.x - firstFloorSleepingBays[i, 0];
                float differenceZ = player.transform.position.z - firstFloorSleepingBays[i, 2];
                float hypoTemp = Mathf.Sqrt((Mathf.Abs(differenceX) * Mathf.Abs(differenceX)) + (Mathf.Abs(differenceZ) * Mathf.Abs(differenceZ)));
                // Check if this distance is shorter than what has already been found
                if (hypoTemp < shortestHypoTemp)
                {
                    shortestHypoTemp = hypoTemp;
                    closestBayTemp = i;
                }
            }

            // Set the loading location
            position = new float[3];
            position[0] = firstFloorSleepingBays[closestBayTemp, 0] + 7;
            position[1] = firstFloorSleepingBays[closestBayTemp, 1];
            position[2] = firstFloorSleepingBays[closestBayTemp, 2] - 5;
        }
        else
        {
            // Second floor
            float shortestHypoTemp = 99999999999.9f;    // Supposed to be a random ridiculously high number
            int closestBayTemp = -1;
            for (int i = 0; i < secondFloorSleepingBays.GetLength(0); i++)
            {
                // Find the hypotenuse (distance between the two points)
                float differenceX = player.transform.position.x - secondFloorSleepingBays[i, 0];
                float differenceZ = player.transform.position.z - secondFloorSleepingBays[i, 2];
                float hypoTemp = Mathf.Sqrt((Mathf.Abs(differenceX) * Mathf.Abs(differenceX)) + (Mathf.Abs(differenceZ) * Mathf.Abs(differenceZ)));
                // Check if this distance is shorter than what has already been found
                if (hypoTemp < shortestHypoTemp)
                {
                    shortestHypoTemp = hypoTemp;
                    closestBayTemp = i;
                }
            }

            position = new float[3];
            position[0] = secondFloorSleepingBays[closestBayTemp, 0] + 500;
            position[1] = secondFloorSleepingBays[closestBayTemp, 1];
            position[2] = secondFloorSleepingBays[closestBayTemp, 2];
        }



    } // End of PlayerData(PlayerMovement player)



}