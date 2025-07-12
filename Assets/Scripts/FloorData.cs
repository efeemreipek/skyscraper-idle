using UnityEngine;

[CreateAssetMenu(fileName = "New FloorData", menuName = "Scriptable Objects/FloorData")]
public class FloorData : ScriptableObject
{
    [Header("Info")]
    public string Name;
    public int BuyCost;

    [Header("Base Stats")]
    public int BaseXPCapAmount = 100;
    public int BaseXPOnClick = 10;
    public int BaseXPOnTime = 2;
    public int BaseMoneyGenerationPerSecond = 1;

    [Header("Growth Multipliers")]
    public float LevelXPMultiplier = 1.25f;
    public float LevelMoneyMultiplier = 1.3f;

    [Header("Visual")]
    public Material NameSignMaterial;
    public Material SymbolMaterial;
}
