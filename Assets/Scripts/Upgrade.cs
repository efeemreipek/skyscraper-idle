using System;
using UnityEngine;

public enum UpgradeType
{
    None,
    IncreaseXPOnClick,
    EnableXPOnTime,
    IncreaseXPOnTime,
    IncreaseMPS
}

public class Upgrade : MonoBehaviour
{
    [SerializeField] private UpgradeType upgradeType;
    [SerializeField] private int currentLevel;
    [SerializeField] private long baseCost;
    [SerializeField] private float costMultiplier;
    [SerializeField] private bool isOneUseOnly;

    private bool canUpgrade = true;
    private long upgradeCost;
    private UpgradeUI ui;
    private bool isInitialized;

    public event Action<Upgrade, UpgradeType> OnUpgradeGathered;

    public long UpgradeCost { get { return upgradeCost; } 
                             set { upgradeCost = value; ui.UpdateCost(upgradeCost); } }

    public bool CanUpgrade => canUpgrade;
    public int CurrentLevel => currentLevel;
    public UpgradeType UpgradeType => upgradeType;
    public bool IsInitialized => isInitialized;

    public void InitializeUpgrade()
    {
        if(isInitialized) return;

        ui = GetComponent<UpgradeUI>();

        if(upgradeCost == 0)
        {
            upgradeCost = baseCost;
        }

        ui.UpdateCost(upgradeCost);

        if(!canUpgrade)
        {
            ui.DisableInteraction();
        }

        isInitialized = true;
    }
    public void UpgradeButton()
    {
        if(!canUpgrade) return;

        AudioManager.Instance.PlayButtonClick();
        MoneyManager.Instance.RemoveMoney(upgradeCost);
        ui.CheckInteractability(upgradeCost);
        currentLevel++;
        upgradeCost = GetUpgradeCost();
        ui.UpdateCost(upgradeCost);
        OnUpgradeGathered?.Invoke(this, upgradeType);

        if(isOneUseOnly)
        { 
            canUpgrade = false;
            ui.DisableInteraction();
        }
    }
    public long GetUpgradeCost()
    {
        double cost = baseCost * Mathf.Pow(costMultiplier, currentLevel);
        return (long)Math.Ceiling(cost);
    }
    public UpgradeSaveData GetSaveData()
    {
        return new UpgradeSaveData()
        {
            UpgradeType = upgradeType,
            CurrentLevel = currentLevel,
            CanUpgrade = canUpgrade
        };
    }
    [ContextMenu("Load")]
    public void Load()
    {
        UpgradeSaveData upgradeSaveData = SaveManager.Instance.SaveData.Floors[0].Upgrades[1];
        LoadFromSave(upgradeSaveData);
    }
    public void LoadFromSave(UpgradeSaveData saveData)
    {
        currentLevel = saveData.CurrentLevel;
        canUpgrade = saveData.CanUpgrade;

        upgradeCost = GetUpgradeCost();

        if(ui == null)
        {
            ui = GetComponent<UpgradeUI>();
        }

        ui.UpdateCost(upgradeCost);

        if(!canUpgrade)
        {
            ui.DisableInteraction();
        }

        isInitialized = true;
    }
}
