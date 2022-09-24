using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    // -- CYNTHIA'S CODE AHEAD, BEWARE -- //
    public static void SavePlayer(PlayerMovement player, LevelManager levelManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.butter";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData playerData = new PlayerData(player, levelManager);

        formatter.Serialize(stream, playerData);
        Debug.Log("Saving data...");
        stream.Close();

    } // End of SavePlayer(PlayerMovement player)

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.butter";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData playerData = formatter.Deserialize(stream) as PlayerData;
            Debug.Log(playerData.position[0]);
            Debug.Log(playerData.position[1]);
            Debug.Log(playerData.position[2]);
            Debug.Log(playerData.doorStatus.Length);
            Debug.Log(playerData.puzzleStatus.Length);
            /*for (int i=0; i<playerData.puzzleStatus.Length; i++)
            {
                Debug.Log(playerData.puzzleStatus[i]);
            }*/
            stream.Close();

            return playerData;
        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }

    } // End of LoadPlayer()


}
