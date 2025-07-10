using System;
using UnityEngine;

public class PrestigeManager : Singleton<PrestigeManager>
{
    private float currentPrestige = 0f;
    private float totalPrestige = 0f;

    private PrestigeManagerUI ui;

    public float Prestige => totalPrestige;

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
        ui.UpdateButton(currentPrestige);
        ui.AddListener(OnButtonClicked);
    }

    private void OnMoneyChanged(long newMoney)
    {
        currentPrestige = Mathf.Max(0f, Mathf.Log10(newMoney) - Mathf.Log10(10_000f));
        ui.UpdateButton(currentPrestige);
    }
    private void OnButtonClicked()
    {
        Debug.Log("Button pressed; prestige:" + currentPrestige);
        totalPrestige += currentPrestige;
        Skyscraper.Instance.ClearSkyscraper();
        MoneyManager.Instance.SetMoneyToStartingMoney();
    }
}
