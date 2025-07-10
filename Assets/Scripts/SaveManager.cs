using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    private SaveData saveData = new SaveData();


    protected override void Awake()
    {
        base.Awake();

        SaveData loaded = SaveSystem.LoadGame();
        if(loaded == null) return;
        saveData = loaded;
    }
    //public void LoadAll()
    //{
    //    SaveData loaded = SaveSystem.LoadGame();
    //    if(loaded == null) return;

    //    saveData = loaded;

    //    LoadSettings(loaded.SettingsData);
    //    LoadMoney(loaded.CurrentMoney);
    //    LoadPrestige(loaded.TotalPrestige, loaded.CurrentPrestige);
    //    LoadFloors(loaded.Floors);
    //}
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
    public void LoadMoney(long money)
    {

    }
    public void SavePrestige()
    {
        if(!PrestigeManager.HasInstance) return;

        saveData.TotalPrestige = PrestigeManager.Instance.TotalPrestige;
        saveData.CurrentPrestige = PrestigeManager.Instance.CurrentPrestige;
    }
    public void LoadPrestige(float totalPrestige, float currentPrestige)
    {

    }
    public void SaveFloors()
    {
        if(!Skyscraper.HasInstance) return;

        saveData.Floors.Clear();

        foreach(Floor floor in Skyscraper.Instance.FloorList)
        {
            saveData.Floors.Add(new FloorSaveData()
            {
                FloorID = floor.Data.Name,
                CurrentLevel = floor.CurrentLevel,
                CurrentXP = floor.CurrentXP,
                HasManager = floor.HasManager,
                ClickXPBoost = floor.ClickXPBoost,
                PassiveXPBoost = floor.PassiveXPBoost,
                MoneyBoost = floor.MoneyBoost
            });
        }
    }
    public void LoadFloors(List<FloorSaveData> floorSaveDatas)
    {

    }

    private void OnApplicationQuit()
    {
        if(!Application.isPlaying) return;

        try
        {
            SaveMoney();
            SavePrestige();
            SaveFloors();

            SaveSystem.SaveGame(saveData);
        }
        catch(Exception e)
        {
            Debug.LogWarning("Save on quit failed: " + e.Message);
        }
    }
}
