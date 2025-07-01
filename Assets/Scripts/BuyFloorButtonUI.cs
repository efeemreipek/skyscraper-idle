using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyFloorButtonUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text mpsText;
    [SerializeField] private TMP_Text buyAmountText;

    private FloorData data;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();    
    }
    private void OnEnable()
    {
        MoneyManager.Instance.OnCurrentMoneyChanged += CheckButtonInteractable;
    }
    private void OnDisable()
    {
        if(MoneyManager.HasInstance) MoneyManager.Instance.OnCurrentMoneyChanged -= CheckButtonInteractable;
    }

    private void CheckButtonInteractable(int amount)
    {
        button.interactable = amount >= data.BuyAmount;
    }
    private void BuyFloor(FloorData data)
    {
        MoneyManager.Instance.RemoveMoney(data.BuyAmount);
        Skyscraper.Instance.AddNewFloor(data);
    }

    public void InitializeButton(FloorData data)
    {
        this.data = data;
        nameText.text = data.Name;
        mpsText.text = $"MPS: ${data.BaseMoneyGenerationPerSecond}";
        buyAmountText.text = $"Amount: ${data.BuyAmount}";
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => BuyFloor(data));
    }
}
