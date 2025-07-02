using UnityEngine;
using UnityEngine.InputSystem;

public class CheatCodes : MonoBehaviour
{
    [SerializeField] private int amount = 1000;

    private void Update()
    {
#if UNITY_EDITOR

        if(Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            MoneyManager.Instance.AddMoney(amount);
        }
        if(Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            MoneyManager.Instance.RemoveMoney(amount);
        }

#endif // #if UNITY_EDITOR
    }
}
