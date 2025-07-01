using System;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] private FloorData data;
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentLevelXP;
    [SerializeField] private int currentXP;
    [SerializeField] private float newLevelXPMultiplier = 1.1f;
    [SerializeField] private int xPGainOnClick = 1;
    [SerializeField] private int moneyGenerationPerSecond = 0;
    [SerializeField] private float newLevelMoneyMultiplier = 1.1f;

    private float timer = 0f;
    private FloorUI ui;

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
        if(currentLevel == 1) return;

        timer += Time.deltaTime;
        if(timer >= 1f)
        {
            timer = 0f;
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
        }
    }

    public void InitializeFloor(FloorData data)
    {
        this.data = data;
        currentLevelXP = data.BaseXPCapAmount;
        newLevelMoneyMultiplier = data.NewLevelMoneyMultiplier;
        ui.SetNameText(data.Name);
    }
}
