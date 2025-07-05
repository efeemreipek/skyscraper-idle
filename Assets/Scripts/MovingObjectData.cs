using UnityEngine;

[CreateAssetMenu(fileName = "New MovingObjectData", menuName = "Scriptable Objects/MovingObjectData")]
public class MovingObjectData : ScriptableObject
{
    public float Speed;
    public float Bound;
}
