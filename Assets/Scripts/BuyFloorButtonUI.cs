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

    public void InitializeButton(FloorData data)
    {
        this.data = data;
        nameText.text = data.Name;
        mpsText.text = $"MPS: ${data.BaseMoneyGenerationPerSecond}";
        buyAmountText.text = $"Amount: ${data.BuyAmount}";
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => Skyscraper.Instance.AddNewFloor(data));
    }
}
