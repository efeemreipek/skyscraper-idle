using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentLevelXP;
    [SerializeField] private int currentXP;
    [SerializeField] private float newLevelMultiplier = 1.1f;
    [SerializeField] private int xPGainOnClick = 1;

    public int XPGainOnClick => xPGainOnClick;

    public void AddXP(int amount)
    {
        currentXP += amount;
        if(currentXP >= currentLevelXP)
        {
            int extraXP = currentXP - currentLevelXP;
            currentLevel++;
            currentXP = extraXP;
            currentLevelXP = Mathf.CeilToInt(currentLevelXP * newLevelMultiplier);
        }
    }
}
