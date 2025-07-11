using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    private SaveData saveData = new SaveData();

    public bool IsFirstTime => saveData.IsFirstTime;
    public SaveData SaveData => saveData;

    protected override void Awake()
    {
        base.Awake();

        SaveData loaded = SaveSystem.LoadGame();
        if(loaded == null) return;
        saveData = loaded;
    }

    public void SaveSettings()
    {
        if(!SettingsManager.HasInstance) return;

        saveData.SettingsData = SettingsManager.Instance.SettingsData;
    }
    public SettingsData LoadSettings()
    {
        return saveData.SettingsData;
    }
    public void SaveMoney()
    {
        if(!MoneyManager.HasInstance) return;

        saveData.CurrentMoney = MoneyManager.Instance.CurrentMoney;
    }
    public long LoadMoney()
    {
        return saveData.CurrentMoney;
    }
    public void SavePrestige()
    {
        if(!PrestigeManager.HasInstance) return;

        saveData.TotalPrestige = PrestigeManager.Instance.TotalPrestige;
        saveData.CurrentPrestige = PrestigeManager.Instance.CurrentPrestige;
    }
    public (float totalPrestige, float currentPrestige) LoadPrestige()
    {
        return (saveData.TotalPrestige, saveData.CurrentPrestige);
    }
    public void SaveFloors()
    {
        if(!Skyscraper.HasInstance) return;

        saveData.Floors.Clear();

        foreach(Floor floor in Skyscraper.Instance.FloorList)
        {
            FloorInfo floorInfo = floor.FloorInfo;

            FloorSaveData floorSaveData = new FloorSaveData()
            {
                FloorID = floor.Data.Name,
                CurrentLevel = floor.CurrentLevel,
                CurrentXP = floor.CurrentXP,
                HasManager = floor.HasManager,
                ClickXPBoost = floor.ClickXPBoost,
                PassiveXPBoost = floor.PassiveXPBoost,
                MoneyBoost = floor.MoneyBoost
            };

            if(floorInfo != null)
            {
                floorSaveData.Upgrades = floorInfo.GetUpgradeSaveData();
            }

            saveData.Floors.Add(floorSaveData);
        }
    }
    public List<FloorSaveData> LoadFloors()
    {
        return saveData.Floors;
    }

    public void DeleteSaveAndReset()
    {
        SaveSystem.DeleteSave();
        ResetSaveData();
    }

    private void ResetSaveData()
    {
        saveData = new SaveData();
    }

    private void OnApplicationQuit()
    {
        if(!Application.isPlaying) return;

        try
        {
            SaveMoney();
            SavePrestige();
            SaveFloors();

            saveData.IsFirstTime = false;
            saveData.LastPlayTime = DateTime.Now.ToBinary();

            SaveSystem.SaveGame(saveData);
        }
        catch(Exception e)
        {
            Debug.LogWarning("Save on quit failed: " + e.Message);
        }
    }
}
