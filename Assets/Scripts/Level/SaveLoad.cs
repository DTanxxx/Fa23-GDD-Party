using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoad : MonoBehaviour
{
    private string saveFilePath = "save.dat";

    // Save the custom struct to a binary file
    public void SaveGame(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = File.Create(GetSavePath());

        formatter.Serialize(fileStream, data);

        fileStream.Close();
    }

    // Load the custom struct from a binary file
    public SaveData LoadGame()
    {
        SaveData data = new SaveData();

        if (File.Exists(GetSavePath()))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = File.Open(GetSavePath(), FileMode.Open);

            data = (SaveData)formatter.Deserialize(fileStream);

            fileStream.Close();
        }

        return data;
    }

    // Get the full path for saving and loading
    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, saveFilePath);
    }
}

