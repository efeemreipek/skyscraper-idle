using UnityEngine;

[CreateAssetMenu(fileName = "New FloorData", menuName = "Scriptable Objects/FloorData")]
public class FloorData : ScriptableObject
{
    [Header("Info")]
    public string Name;
    public int BuyCost;
    public int BaseXPCapAmount;
    public int BaseMoneyGenerationPerSecond;
    [Header("Multipliers")]
    public float NewLevelMoneyMultiplier;
}
