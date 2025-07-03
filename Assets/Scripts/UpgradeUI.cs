using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private TMP_Text costText;

    private Upgrade upgrade;

    public Button Button => button;

    private void OnEnable()
    {
        MoneyManager.Instance.OnCurrentMoneyChanged += OnCurrentMoneyChanged;
    }
    private void OnDisable()
    {
        if(MoneyManager.HasInstance) MoneyManager.Instance.OnCurrentMoneyChanged -= OnCurrentMoneyChanged;
    }
    private void Awake()
    {
        upgrade = GetComponent<Upgrade>();
    }

    public void UpdateCost(int cost)
    {
        costText.text = $"$ {cost}";
        CheckInteractability(cost);
    }

    public void CheckInteractability(int cost)
    {
        button.interactable = MoneyManager.Instance.CurrentMoney >= cost;
    }
    private void OnCurrentMoneyChanged(int currentMoney)
    {
        CheckInteractability(upgrade.UpgradeCost);
    }
}
