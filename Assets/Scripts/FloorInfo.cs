using System;
using System.Collections.Generic;
using UnityEngine;

public class FloorInfo : MonoBehaviour
{
    [SerializeField] private Upgrade[] upgrades = new Upgrade[4];

    private Floor floor;
    private FloorInfoUI ui;

    private void OnEnable()
    {
        for(int i = 0; i < upgrades.Length; i++)
        {
            Upgrade upgrade = upgrades[i];

            upgrade.OnUpgradeGathered += OnUpgradeGathered;
        }
    }
    private void OnDisable()
    {
        for(int i = 0; i < upgrades.Length; i++)
        {
            Upgrade upgrade = upgrades[i];

            upgrade.OnUpgradeGathered -= OnUpgradeGathered;
        }

        floor.OnFloorGainedXP -= OnFloorGainedXP;
        floor.OnFloorLeveledUp -= OnFloorLeveledUp;
    }
    private void Awake()
    {
        ui = GetComponent<FloorInfoUI>();
    }

    public void InitializeFloorInfo(Floor floor)
    {
        this.floor = floor;

        this.floor.OnFloorGainedXP += OnFloorGainedXP;
        this.floor.OnFloorLeveledUp += OnFloorLeveledUp;

        floor.FloorInfo = this;

        ui.InitializePanel(floor);

        for(int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i].InitializeUpgrade();
        }
    }
    public List<UpgradeSaveData> GetUpgradeSaveData()
    {
        List<UpgradeSaveData> upgradeData = new List<UpgradeSaveData>();

        for(int i = 0; i < upgrades.Length; i++)
        {
            upgradeData.Add(upgrades[i].GetSaveData());
        }

        return upgradeData;
    }

    public void LoadUpgrades(List<UpgradeSaveData> upgradeData)
    {
        for(int i = 0; i < upgradeData.Count && i < upgrades.Length; i++)
        {
            // Find the matching upgrade by type
            for(int j = 0; j < upgrades.Length; j++)
            {
                if(upgrades[j].UpgradeType == upgradeData[i].UpgradeType)
                {
                    upgrades[j].LoadFromSave(upgradeData[i]);
                    break;
                }
            }
        }

        // Initialize any upgrades that weren't loaded from save
        for(int i = 0; i < upgrades.Length; i++)
        {
            if(!upgrades[i].IsInitialized)
            {
                upgrades[i].InitializeUpgrade();
            }
        }
    }


    private void OnUpgradeGathered(Upgrade upgrade, UpgradeType upgradeType)
    {
        floor.AcceptUpgrade(upgrade, upgradeType);
        ui.UpdatePanel(floor.CurrentLevel, floor.CurrentMoneyPerSecond);
    }
    private void OnFloorGainedXP(float currentProgress)
    {
        ui.UpdateXPBar(currentProgress);
    }
    private void OnFloorLeveledUp(int newLevel, long mps)
    {
        ui.UpdatePanel(newLevel, mps);
    }
}
