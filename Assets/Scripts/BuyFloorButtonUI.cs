using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyFloorButtonUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;

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
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => Skyscraper.Instance.AddNewFloor(data));
    }
}
