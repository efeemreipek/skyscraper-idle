using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [SerializeField] private MovingObjectData data;
    [SerializeField, Tooltip("0 is left, 1 or anything else is right")] private int direction; // 1: right 0: left

    private void Update()
    {
        transform.Translate(data.Speed * Time.deltaTime * (direction == 0 ? -transform.right : transform.right), Space.Self);

        if(transform.position.x < -data.Bound)
        {
            transform.position = new Vector3(data.Bound, transform.position.y, transform.position.z);
        }
        if(transform.position.x > data.Bound)
        {
            transform.position = new Vector3(-data.Bound, transform.position.y, transform.position.z);
        }
    }
}
