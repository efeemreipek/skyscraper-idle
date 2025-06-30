using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FloorDataList", menuName = "Scriptable Objects/FloorDataList")]
public class FloorDataList : ScriptableObject
{
    public List<FloorData> List = new List<FloorData>();
}
