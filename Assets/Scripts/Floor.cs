using System;
using System.Collections;
using System.Collections.Generic;
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

    [Header("Visual")]
    [SerializeField] private MeshRenderer floorMeshRenderer;
    [SerializeField] private MeshRenderer symbolMeshRenderer;

    private FloorUI ui;

    public event Action<int, long> OnFloorLeveledUp;
    public event Action<float> OnFloorGainedXP;

    public FloorData Data => data;
    public int CurrentLevel => currentLevel;
    public int CurrentXP => currentXP;
    public float CurrentLevelProgress => (float)currentXP / GetXPRequiredForLevel(currentLevel);
    public long CurrentMoneyPerSecond => currentLevel >= 2 ? GetMoneyPerSecond() : 0;
    public bool HasManager => hasManager;
    public float ClickXPBoost => clickXPBoost;
    public float PassiveXPBoost => passiveXPBoost;
    public float MoneyBoost => moneyBoost;
    public FloorInfo FloorInfo { get; set; }

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

        floorMeshRenderer.material = data.NameSignMaterial;
        symbolMeshRenderer.material = data.SymbolMaterial;

        ui.SetNameText(data.Name);
    }
    private void ResetUpgrades()
    {
        float prestige = PrestigeManager.Instance.TotalPrestige;

        hasManager = false;
        clickXPBoost = 1f + prestige;
        passiveXPBoost = 1f + prestige;
        moneyBoost = 1f + prestige;
    }
    public void AcceptUpgrade(Upgrade upgrade, UpgradeType upgradeType)
    {
        float prestige = PrestigeManager.Instance.TotalPrestige;

        switch(upgradeType)
        {
            case UpgradeType.None:
                break;
            case UpgradeType.IncreaseXPOnClick:
                clickXPBoost = (1f + Mathf.Log10(upgrade.CurrentLevel + 1f) * 5f) + prestige;
                break;
            case UpgradeType.EnableXPOnTime:
                hasManager = true;
                break;
            case UpgradeType.IncreaseXPOnTime:
                passiveXPBoost = (1f + Mathf.Log10(upgrade.CurrentLevel + 1f) * 3.5f) + prestige;
                break;
            case UpgradeType.IncreaseMPS:
                moneyBoost = (1f + Mathf.Log10(upgrade.CurrentLevel + 1f) * 4f) + prestige;
                break;
            default:
                break;
        }
    }
    public void LoadFromSave(FloorSaveData data)
    {
        currentLevel = data.CurrentLevel;
        currentXP = data.CurrentXP;
        hasManager = data.HasManager;
        clickXPBoost = data.ClickXPBoost;
        passiveXPBoost = data.PassiveXPBoost;
        moneyBoost = data.MoneyBoost;

        if(data.Upgrades != null && data.Upgrades.Count > 0)
        {
            if(FloorInfo != null)
            {
                FloorInfo.LoadUpgrades(data.Upgrades);
            }
            else
            {
                StartCoroutine(LoadUpgradesWhenReady(data.Upgrades));
            }
        }

        OnFloorLeveledUp?.Invoke(currentLevel, CurrentMoneyPerSecond);
        OnFloorGainedXP?.Invoke(CurrentLevelProgress);
    }
    private IEnumerator LoadUpgradesWhenReady(List<UpgradeSaveData> upgradeData)
    {
        // Wait until FloorInfo is set
        while(FloorInfo == null)
        {
            yield return null;
        }

        FloorInfo.LoadUpgrades(upgradeData);
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
