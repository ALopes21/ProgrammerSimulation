using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Persistence : MonoBehaviour
{
    public static Persistence control;
    public static List<LevelData> savedLevels = new List<LevelData>();

    public void Awake()
    {
        if(control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if(control != this)
        {
            Destroy(gameObject);
        }
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedLevels.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedLevels.dat", FileMode.Open);
            savedLevels = (List<LevelData>)bf.Deserialize(file);
            Debug.Log(savedLevels.Count);
            file.Close();
        }
    }

    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedLevels.dat");
        Debug.Log("File: " + file.Name);
        bf.Serialize(file, savedLevels);
        file.Close();
    }

    public static void UpdateLevelList(LevelData currentLevel)
    {
        for (int i = 0; i < savedLevels.Count; i++)
        {
            if (savedLevels[i].Number == currentLevel.Number)
            {
                savedLevels[i] = currentLevel;
                Save();
                return;
            }
        }

        savedLevels.Add(currentLevel);
        Debug.Log(currentLevel.Number + " : " + currentLevel.Time);
        Save();
    }
}

[System.Serializable]
public class LevelData
{
    public static LevelData current;
    public int Number;
    public int Stars;
    public float Time;
    public bool Unclocked;

}

