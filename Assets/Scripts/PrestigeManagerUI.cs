using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrestigeManagerUI : MonoBehaviour
{
    [SerializeField] private Button sellSkyscraperButton;
    [SerializeField] private TMP_Text sellSkyscraperText;

    private readonly string buttonText = "<size=50>SELL SKYSCRAPER</size>\r\nPRESTIGE GAINED: ";

    public void UpdateButton(float prestige)
    {
        sellSkyscraperText.text = buttonText + prestige.ToString("F2");
        sellSkyscraperButton.interactable = prestige >= 1f;
    }
    public void AddListener(UnityEngine.Events.UnityAction action)
    {
        sellSkyscraperButton.onClick.RemoveAllListeners();
        sellSkyscraperButton.onClick.AddListener(action);
    }
}
