using TMPro;
using UnityEngine;

public class BuyFloorButtonUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;

    public void InitializeButton(string name)
    {
        nameText.text = name;
    }
}
