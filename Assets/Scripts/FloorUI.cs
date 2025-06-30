using TMPro;
using UnityEngine;

public class FloorUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;

    public void SetNameText(string text)
    {
        nameText.text = text;
    }
}
