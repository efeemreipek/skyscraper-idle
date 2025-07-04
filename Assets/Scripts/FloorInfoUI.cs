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

    private RectTransform scrollViewContentRectTransform;

    private void Start()
    {
        scrollViewContentRectTransform = GetComponentInParent<RectTransform>();
    }

    public void InitializePanel(Floor floor)
    {
        nameText.text = floor.Data.Name;
        UpdatePanel(floor);
    }
    public void UpdatePanel(Floor floor)
    {
        mpsText.text = $"$ {floor.MoneyGenerationPerSecond}/sec";
        levelText.text = $"Level {floor.CurrentLevel}";
        UpdateXPBar(floor.CurrentLevelProgress);
    }
    public void UpdatePanel(int newLevel, int mps)
    {
        mpsText.text = $"$ {mps}/sec";
        levelText.text = $"Level {newLevel}";
    }
    public void UpdateXPBar(float currentProgress)
    {
        levelProgressImage.fillAmount = currentProgress;
    }

    public void OpenCloseUpgrades()
    {
        upgradesContainer.SetActive(!upgradesContainer.activeSelf);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)upgradesContainer.transform); 
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollViewContentRectTransform);
    }
}
