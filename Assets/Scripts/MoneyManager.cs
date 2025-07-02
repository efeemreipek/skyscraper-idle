using System;
using UnityEngine;

public class MoneyManager : Singleton<MoneyManager>
{
    [SerializeField] private int startingMoney = 10;

    private MoneyManagerUI ui;

    public event Action<int> OnCurrentMoneyChanged;
    public int CurrentMoney;

    protected override void Awake()
    {
        base.Awake();

        ui = GetComponent<MoneyManagerUI>();
    }
    private void Start()
    {
        ChangeCurrentMoneyTo(startingMoney);
    }

    private void ChangeCurrentMoneyTo(int amount)
    {
        CurrentMoney = amount < 0 ? 0 : amount;
        OnCurrentMoneyChanged?.Invoke(CurrentMoney);
        ui.UpdateText(CurrentMoney);
    }
    public void AddMoney(int amount)
    {
        if(amount == 0) return;
        ChangeCurrentMoneyTo(CurrentMoney + amount);
    }
    public void RemoveMoney(int amount)
    {
        if(amount == 0) return;
        ChangeCurrentMoneyTo(CurrentMoney - amount);
    }
}
