using System;
using UnityEngine;

public class MoneyManager : Singleton<MoneyManager>
{
    [SerializeField] private long startingMoney = 10;

    private MoneyManagerUI ui;

    public event Action<long> OnCurrentMoneyChanged;
    public long CurrentMoney;

    protected override void Awake()
    {
        base.Awake();

        ui = GetComponent<MoneyManagerUI>();
    }
    private void Start()
    {
        SetMoneyToStartingMoney();
    }

    private void ChangeCurrentMoneyTo(long amount)
    {
        CurrentMoney = amount < 0 ? 0 : amount;
        OnCurrentMoneyChanged?.Invoke(CurrentMoney);
        ui.UpdateText(CurrentMoney);
    }
    public void AddMoney(long amount)
    {
        if(amount == 0) return;
        ChangeCurrentMoneyTo(CurrentMoney + amount);
    }
    public void RemoveMoney(long amount)
    {
        if(amount == 0) return;
        ChangeCurrentMoneyTo(CurrentMoney - amount);
    }
    public void SetMoneyToStartingMoney()
    {
        ChangeCurrentMoneyTo(startingMoney);
    }
}
