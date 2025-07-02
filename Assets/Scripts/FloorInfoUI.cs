using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloorInfoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text mpsText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Image levelProgressImage;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TMP_Text upgradeButtonText;

    private Floor floor;

    private void OnEnable()
    {
        MoneyManager.Instance.OnCurrentMoneyChanged += OnCurrentMoneyChanged;
    }
    private void OnDisable()
    {
        if(floor != null) floor.OnFloorLeveledUp -= OnFloorLeveledUp;
        if(MoneyManager.HasInstance) MoneyManager.Instance.OnCurrentMoneyChanged -= OnCurrentMoneyChanged;
    }

    public void InitializePanel(Floor floor)
    {
        this.floor = floor;

        floor.OnFloorLeveledUp += OnFloorLeveledUp;

        nameText.text = floor.Data.Name;
        InitializeUpgradeButton();
        UpdatePanel();
    }
    public void UpdatePanel()
    {
        mpsText.text = $"$ {floor.MoneyGenerationPerSecond}/sec";
        levelText.text = $"Level {floor.CurrentLevel}";
        levelProgressImage.fillAmount = floor.CurrentLevelProgress;
    }

    private void OnFloorLeveledUp(int newLevel)
    {
        upgradeButtonText.text = $"UPGRADE ${floor.UpgradeCost}";
    }
    private void OnCurrentMoneyChanged(int currentMoney)
    {
        CheckUpgradeButtonInteractable(currentMoney);
    }
    private void CheckUpgradeButtonInteractable(int amount)
    {
        upgradeButton.interactable = amount >= floor.UpgradeCost;
    }
    private void InitializeUpgradeButton()
    {
        CheckUpgradeButtonInteractable(MoneyManager.Instance.CurrentMoney);

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() => MoneyManager.Instance.RemoveMoney(floor.UpgradeCost));

        upgradeButtonText.text = $"UPGRADE ${floor.UpgradeCost}";
    }
}
