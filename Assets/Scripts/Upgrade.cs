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
    [SerializeField] private bool isOneUseOnly;
    [SerializeField] private UpgradeType upgradeType;

    private bool canUpgrade = true;
    private long upgradeCost;
    private UpgradeUI ui;

    public event Action<Upgrade, UpgradeType> OnUpgradeGathered;

    public long UpgradeCost { get { return upgradeCost; } 
                             set { upgradeCost = value; ui.UpdateCost(upgradeCost); } }

    public bool CanUpgrade => canUpgrade;

    public void InitializeUpgrade(long cost)
    {
        ui = GetComponent<UpgradeUI>();

        upgradeCost = cost;
        ui.UpdateCost(upgradeCost);
    }
    public void UpgradeButton()
    {
        if(!canUpgrade) return;

        AudioManager.Instance.PlayButtonClick(0.7f, false);
        MoneyManager.Instance.RemoveMoney(upgradeCost);
        ui.CheckInteractability(upgradeCost);
        OnUpgradeGathered?.Invoke(this, upgradeType);

        if(isOneUseOnly)
        { 
            canUpgrade = false;
            ui.DisableInteraction();
        }
    }
}
