using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float bound;
    [SerializeField, Tooltip("0 is left, 1 or anything else is right")] private int direction; // 1: right 0: left

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * (direction == 0 ? -transform.right : transform.right), Space.Self);

        if(transform.position.x < -bound)
        {
            transform.position = new Vector3(bound, transform.position.y, transform.position.z);
        }
        if(transform.position.x > bound)
        {
            transform.position = new Vector3(-bound, transform.position.y, transform.position.z);
        }
    }
}
