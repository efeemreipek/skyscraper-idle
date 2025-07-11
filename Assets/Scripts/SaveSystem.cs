using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public bool IsFirstTime = true;
    public SettingsData SettingsData;
    public float TotalPrestige;
    public float CurrentPrestige;
    public long CurrentMoney;
    public List<FloorSaveData> Floors = new List<FloorSaveData>();
}

[System.Serializable]
public class SettingsData
{
    public float MusicVolume = 1f;
    public float SFXVolume = 1f;
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
    public List<UpgradeSaveData> Upgrades = new List<UpgradeSaveData>();
}

[System.Serializable]
public class UpgradeSaveData
{
    public UpgradeType UpgradeType;
    public int CurrentLevel;
    public bool CanUpgrade;
}

public static class SaveSystem
{
    private static readonly string savePath = Application.persistentDataPath + "/save.json";

    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }
    public static SaveData LoadGame()
    {
        if(!File.Exists(savePath))
        {
            return null;
        }

        string json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<SaveData>(json);
    }
    public static void DeleteSave()
    {
        if(File.Exists(savePath))
        {
            File.Delete(savePath);
        }
    }
}
