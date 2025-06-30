using System;
using TMPro;
using UnityEngine;

public class MoneyManagerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text currentMoneyText;

    public void UpdateText(int currentMoney)
    {
        currentMoneyText.text = currentMoney.ToString();
    }
}
