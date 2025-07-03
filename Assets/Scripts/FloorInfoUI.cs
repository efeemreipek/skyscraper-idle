using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloorInfoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text mpsText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Image levelProgressImage;
    [SerializeField] private GameObject upgradesContainer;

    private Floor floor;
    private RectTransform scrollViewContentRectTransform;

    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {

    }
    private void Start()
    {
        scrollViewContentRectTransform = GetComponentInParent<RectTransform>();
    }

    public void InitializePanel(Floor floor)
    {
        this.floor = floor;

        nameText.text = floor.Data.Name;
        UpdatePanel();
    }
    public void UpdatePanel()
    {
        mpsText.text = $"$ {floor.MoneyGenerationPerSecond}/sec";
        levelText.text = $"Level {floor.CurrentLevel}";
        levelProgressImage.fillAmount = floor.CurrentLevelProgress;
    }

    public void OpenCloseUpgrades()
    {
        upgradesContainer.SetActive(!upgradesContainer.activeSelf);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)upgradesContainer.transform); 
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollViewContentRectTransform);
    }
}
