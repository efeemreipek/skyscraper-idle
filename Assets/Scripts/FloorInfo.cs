using UnityEngine;

public class FloorInfo : MonoBehaviour
{
    [SerializeField] private Upgrade[] upgrades = new Upgrade[4];

    private Floor floor;
    private FloorInfoUI ui;

    private void OnEnable()
    {
        foreach(Upgrade upgrade in upgrades)
        {
            upgrade.OnUpgradeGathered += OnUpgradeGathered;
        }
    }
    private void OnDisable()
    {
        foreach(Upgrade upgrade in upgrades)
        {
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
        
    }
}
