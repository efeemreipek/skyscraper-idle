using System;
using UnityEngine;

public class FloorInfo : MonoBehaviour
{
    [SerializeField] private Upgrade[] upgrades = new Upgrade[4];
    [SerializeField] private int[] upgradeCosts = new int[4];
    [SerializeField] private float upgradeCostMultiplier = 1.25f;

    private Floor floor;
    private FloorInfoUI ui;

    private void OnEnable()
    {
        for(int i = 0; i < upgrades.Length; i++)
        {
            Upgrade upgrade = upgrades[i];

            upgrade.OnUpgradeGathered += OnUpgradeGathered;
            upgrade.InitializeUpgrade(upgradeCosts[i]);
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

        ui.InitializePanel(floor);
    }

    private void OnUpgradeGathered(Upgrade upgrade, UpgradeType upgradeType)
    {
        floor.AcceptUpgrade(upgradeType);
        ui.UpdatePanel(floor.CurrentLevel, floor.MoneyGenerationPerSecond);

        for(int i = 0; i < upgrades.Length; i++)
        {
            if(upgrades[i] == upgrade)
            {
                upgradeCosts[i] = Mathf.CeilToInt(upgradeCosts[i] * upgradeCostMultiplier);
                upgrades[i].UpgradeCost = upgradeCosts[i];
            }
        }
    }
    private void OnFloorGainedXP(float currentProgress)
    {
        ui.UpdateXPBar(currentProgress);
    }
    private void OnFloorLeveledUp(int newLevel, int mps)
    {
        ui.UpdatePanel(newLevel, mps);
    }
}
