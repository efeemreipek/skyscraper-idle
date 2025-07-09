using System;
using UnityEngine;

public class PrestigeManager : Singleton<PrestigeManager>
{
    private float prestige = 0f;

    private PrestigeManagerUI ui;

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
        ui.UpdateButton(prestige);
    }

    private void OnMoneyChanged(long newMoney)
    {
        prestige = Mathf.Max(0f, Mathf.Log10(newMoney) - Mathf.Log10(10_000f));
        ui.UpdateButton(prestige);
    }
}
