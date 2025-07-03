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
    [SerializeField] private int moneyGenerationPerSecond = 0;
    [Header("Multipliers")]
    [SerializeField] private float newLevelXPMultiplier = 1.1f;
    [SerializeField] private float newLevelMoneyMultiplier = 1.1f;

    private float timer = 0f;
    private bool canGainXPOnTime;
    private FloorUI ui;

    public event Action<int> OnFloorLeveledUp;

    public FloorData Data => data;
    public int CurrentLevel => currentLevel;
    public int MoneyGenerationPerSecond => moneyGenerationPerSecond;
    public float CurrentLevelProgress => (float)currentXP / currentLevelXP;
    public int XPGainOnClick => xPGainOnClick;

    private void Awake()
    {
        ui = GetComponent<FloorUI>();
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 1f)
        {
            timer = 0f;

            if(canGainXPOnTime)
            {
                AddXP(xPGainOnTime);
            }

            MoneyManager.Instance.AddMoney(moneyGenerationPerSecond);
        }
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
                moneyGenerationPerSecond = Mathf.CeilToInt(moneyGenerationPerSecond * newLevelMoneyMultiplier);
            }

            OnFloorLeveledUp?.Invoke(currentLevel);
        }
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
                moneyGenerationPerSecond = Mathf.CeilToInt(moneyGenerationPerSecond * newLevelMoneyMultiplier);
                break;
            default:
                break;
        }
    }
}
