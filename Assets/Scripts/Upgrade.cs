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
    private UpgradeUI ui;

    public event Action<Upgrade, UpgradeType> OnUpgradeGathered;

    private void Awake()
    {
        ui = GetComponent<UpgradeUI>();
    }

    public void UpgradeButton()
    {
        if(!canUpgrade) return;

        OnUpgradeGathered?.Invoke(this, upgradeType);

        if(isOneUseOnly) canUpgrade = false;
        ui.Button.interactable = canUpgrade;
    }
}
