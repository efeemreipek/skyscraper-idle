using System;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private FloorData data;
    [Header("Info")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentLevelXP;
    [SerializeField] private int currentXP;
    [SerializeField] private int xPGainOnClick = 1;
    [SerializeField] private int xPGainOnTime = 1;
    [SerializeField] private long moneyGenerationPerSecond = 0;
    [Header("Multipliers")]
    [SerializeField] private float newLevelXPMultiplier = 1.1f;
    [SerializeField] private float newLevelMoneyMultiplier = 1.1f;

    private bool canGainXPOnTime;
    private FloorUI ui;

    public event Action<int, long> OnFloorLeveledUp;
    public event Action<float> OnFloorGainedXP;

    public FloorData Data => data;
    public int CurrentLevel => currentLevel;
    public long MoneyGenerationPerSecond => moneyGenerationPerSecond;
    public float CurrentLevelProgress => (float)currentXP / currentLevelXP;
    public int XPGainOnClick => xPGainOnClick;

    private void OnEnable()
    {
        TickManager.Instance.AddToList(this);
    }
    private void OnDisable()
    {
        if(TickManager.HasInstance)
        {
            TickManager.Instance.RemoveFromList(this);
        }
    }
    private void Awake()
    {
        ui = GetComponent<FloorUI>();
    }

    public bool OnTick()
    {
        if(canGainXPOnTime)
        {
            AddXP(xPGainOnTime);
        }

        if(moneyGenerationPerSecond > 0)
        {
            MoneyManager.Instance.AddMoney(moneyGenerationPerSecond);
            return true;
        }

        return false;
    }
    public void AddXP(int amount)
    {
        currentXP += amount;
        if(currentXP >= currentLevelXP)
        {
            int extraXP = currentXP - currentLevelXP;
            currentLevel++;
            currentXP = extraXP;
            currentLevelXP = Mathf.CeilToInt(currentLevelXP * newLevelXPMultiplier);

            if(currentLevel == 2)
            {
                moneyGenerationPerSecond = data.BaseMoneyGenerationPerSecond;
            }
            else
            {
                double newMPS = moneyGenerationPerSecond * newLevelMoneyMultiplier;
                moneyGenerationPerSecond = (long)Math.Ceiling(newMPS);
            }

            OnFloorLeveledUp?.Invoke(currentLevel, moneyGenerationPerSecond);
        }
        OnFloorGainedXP?.Invoke(CurrentLevelProgress);
    }
    public void InitializeFloor(FloorData data)
    {
        this.data = data;
        currentLevelXP = data.BaseXPCapAmount;
        newLevelMoneyMultiplier = data.NewLevelMoneyMultiplier;
        ui.SetNameText(data.Name);
    }
    public void AcceptUpgrade(UpgradeType upgradeType)
    {
        switch(upgradeType)
        {
            case UpgradeType.None:
                break;
            case UpgradeType.IncreaseXPOnClick:
                Debug.Log($"{data.Name} has gathered the upgrade: {upgradeType}");
                xPGainOnClick++;
                break;
            case UpgradeType.EnableXPOnTime:
                Debug.Log($"{data.Name} has gathered the upgrade: {upgradeType}");
                canGainXPOnTime = true;
                break;
            case UpgradeType.IncreaseXPOnTime:
                Debug.Log($"{data.Name} has gathered the upgrade: {upgradeType}");
                xPGainOnTime++;
                break;
            case UpgradeType.IncreaseMPS:
                Debug.Log($"{data.Name} has gathered the upgrade: {upgradeType}");
                double newMPS = moneyGenerationPerSecond * newLevelMoneyMultiplier;
                moneyGenerationPerSecond = (long)Math.Ceiling(newMPS);
                break;
            default:
                break;
        }
    }
}
