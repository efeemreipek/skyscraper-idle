using UnityEngine;

public class MoneyManager : Singleton<MoneyManager>
{
    public int CurrentMoney;

    private MoneyManagerUI ui;

    protected override void Awake()
    {
        base.Awake();

        ui = GetComponent<MoneyManagerUI>();

        ui.UpdateText(CurrentMoney);
    }

    public void AddMoney(int amount)
    {
        CurrentMoney += amount;
        ui.UpdateText(CurrentMoney);
    }
}
