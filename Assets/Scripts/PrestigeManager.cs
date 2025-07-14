using System;
using UnityEngine;

public class PrestigeManager : Singleton<PrestigeManager>
{
    private float currentPrestige = 0f;
    private float totalPrestige = 0f;

    private PrestigeManagerUI ui;

    public float TotalPrestige => totalPrestige;
    public float CurrentPrestige => currentPrestige;

    private void OnEnable()
    {
        MoneyManager.Instance.OnCurrentMoneyChanged += OnMoneyChanged;   
    }
    private void OnDisable()
    {
        if(MoneyManager.HasInstance) MoneyManager.Instance.OnCurrentMoneyChanged -= OnMoneyChanged;
    }
    protected override void Awake()
    {
        base.Awake();

        ui = GetComponent<PrestigeManagerUI>();
        ui.AddListener(OnButtonClicked);

        var prestige = SaveManager.Instance.LoadPrestige();
        totalPrestige = prestige.totalPrestige;
        currentPrestige = prestige.currentPrestige;

        ui.UpdateButton(currentPrestige);
    }

    private void OnMoneyChanged(long newMoney)
    {
        currentPrestige = Mathf.Max(0f, Mathf.Log10(newMoney) - Mathf.Log10(10_000f));
        ui.UpdateButton(currentPrestige);
    }
    private void OnButtonClicked()
    {
        totalPrestige += currentPrestige;
        Skyscraper.Instance.ClearSkyscraper();
        MoneyManager.Instance.SetMoneyToStartingMoney();
    }
}
