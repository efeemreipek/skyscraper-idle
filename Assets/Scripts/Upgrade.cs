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

    public event Action<Upgrade, UpgradeType> OnUpgradeGathered;

    public long UpgradeCost { get { return upgradeCost; } 
                             set { upgradeCost = value; ui.UpdateCost(upgradeCost); } }

    public bool CanUpgrade => canUpgrade;
    public int CurrentLevel => currentLevel;

    public void InitializeUpgrade()
    {
        ui = GetComponent<UpgradeUI>();

        upgradeCost = baseCost;
        ui.UpdateCost(upgradeCost);
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
}
