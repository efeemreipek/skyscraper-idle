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
    }
    private void Awake()
    {
        ui = GetComponent<FloorInfoUI>();
    }

    public void InitializeFloorInfo(Floor floor)
    {
        this.floor = floor;
        ui.InitializePanel(floor);
    }
    public void UpdateFloorInfo()
    {
        ui.UpdatePanel(floor);
    }

    private void OnUpgradeGathered(Upgrade upgrade, UpgradeType upgradeType)
    {
        floor.AcceptUpgrade(upgradeType);

        for(int i = 0; i < upgrades.Length; i++)
        {
            if(upgrades[i] == upgrade)
            {
                upgradeCosts[i] = Mathf.CeilToInt(upgradeCosts[i] * upgradeCostMultiplier);
                upgrades[i].UpgradeCost = upgradeCosts[i];
            }
        }
    }
}
