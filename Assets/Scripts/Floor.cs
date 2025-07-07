using System;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private FloorData data;

    [Header("Progress")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentXP = 0;

    [Header("Upgrades")]
    [SerializeField] private bool hasManager = false;
    [SerializeField] private float clickXPBoost = 1f;
    [SerializeField] private float passiveXPBoost = 1f;
    [SerializeField] private float moneyBoost = 1f;

    private FloorUI ui;

    public event Action<int, long> OnFloorLeveledUp;
    public event Action<float> OnFloorGainedXP;

    public FloorData Data => data;
    public int CurrentLevel => currentLevel;
    public float CurrentLevelProgress => (float)currentXP / GetXPRequiredForLevel(currentLevel);
    public long CurrentMoneyPerSecond => currentLevel >= 2 ? GetMoneyPerSecond() : 0;

    private void OnEnable()
    {
        TickManager.Instance.AddToList(this);
    }
    private void OnDisable()
    {
        if(TickManager.HasInstance) TickManager.Instance.RemoveFromList(this);
    }
    private void Awake()
    {
        ui = GetComponent<FloorUI>();
    }

    public bool OnTick()
    {
        if(hasManager)
        {
            AddXP(GetXPOnTime());
        }

        if(CurrentMoneyPerSecond > 0)
        {
            MoneyManager.Instance.AddMoney(CurrentMoneyPerSecond);
            return true;
        }

        return false;
    }
    public void AddXP(int amount)
    {
        currentXP += amount;

        while(currentXP >= GetXPRequiredForLevel(currentLevel))
        {
            currentXP -= GetXPRequiredForLevel(currentLevel);
            currentLevel++;

            OnFloorLeveledUp?.Invoke(currentLevel, CurrentMoneyPerSecond);
        }

        OnFloorGainedXP?.Invoke(CurrentLevelProgress);
    }
    public void AddXPByClick()
    {
        AddXP(GetXPOnClick());
    }
    public void InitializeFloor(FloorData data)
    {
        this.data = data;

        currentLevel = 1;
        currentXP = 0;
        ResetUpgrades();

        ui.SetNameText(data.Name);
    }
    private void ResetUpgrades()
    {
        hasManager = false;
        clickXPBoost = 1f;
        passiveXPBoost = 1f;
        moneyBoost = 1f;
    }
    public void AcceptUpgrade(Upgrade upgrade, UpgradeType upgradeType)
    {
        switch(upgradeType)
        {
            case UpgradeType.None:
                break;
            case UpgradeType.IncreaseXPOnClick:
                clickXPBoost = 1f + Mathf.Log10(upgrade.CurrentLevel + 1f) * 5f;
                break;
            case UpgradeType.EnableXPOnTime:
                hasManager = true;
                break;
            case UpgradeType.IncreaseXPOnTime:
                passiveXPBoost = 1f + Mathf.Log10(upgrade.CurrentLevel + 1f) * 3.5f;
                break;
            case UpgradeType.IncreaseMPS:
                moneyBoost = 1f + Mathf.Log10(upgrade.CurrentLevel + 1f) * 4f;
                break;
            default:
                break;
        }
    }

    private int GetXPOnClick() => Mathf.CeilToInt(data.BaseXPOnClick * clickXPBoost);
    private int GetXPOnTime() => Mathf.CeilToInt(data.BaseXPOnTime * passiveXPBoost);
    private int GetXPRequiredForLevel(int level) => Mathf.CeilToInt(data.BaseXPCapAmount * Mathf.Pow(data.LevelXPMultiplier, level - 1));
    private long GetMoneyPerSecond()
    {
        double baseMPS = data.BaseMoneyGenerationPerSecond * Mathf.Pow(data.LevelMoneyMultiplier, currentLevel - 2);
        return (long)Math.Ceiling(baseMPS * moneyBoost);
    }
}
