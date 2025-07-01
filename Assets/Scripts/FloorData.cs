using UnityEngine;

[CreateAssetMenu(fileName = "New FloorData", menuName = "Scriptable Objects/FloorData")]
public class FloorData : ScriptableObject
{
    public string Name;
    public int BaseXPCapAmount;
    public int BaseMoneyGenerationPerSecond;
    public float NewLevelMoneyMultiplier;
    public int BuyAmount;
}
