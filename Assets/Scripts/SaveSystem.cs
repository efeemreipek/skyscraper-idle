using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public SettingsData SettingsData;
    public float TotalPrestige;
    public float CurrentPrestige;
    public long CurrentMoney;
    public List<FloorSaveData> Floors = new List<FloorSaveData>();
}

[System.Serializable]
public class SettingsData
{
    public float MusicVolume;
    public float SFXVolume;
    public int ResolutionIndex;
    public int FullScreenIndex;
    public int QualityIndex;
    public bool IsVSyncOn;
}

[System.Serializable]
public class FloorSaveData
{
    public string FloorID;
    public int CurrentLevel;
    public int CurrentXP;
    public bool HasManager;
    public float ClickXPBoost;
    public float PassiveXPBoost;
    public float MoneyBoost;
}

public static class SaveSystem
{
    private static readonly string savePath = Application.persistentDataPath + "/save.json";

    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game saved to: " + savePath);
    }
    public static SaveData LoadGame()
    {
        if(!File.Exists(savePath))
        {
            Debug.Log("No save file found");
            return null;
        }

        string json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<SaveData>(json);
    }
}
