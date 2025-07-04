using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSensitivity = 2f;
    [SerializeField] private float lowestClamp = 0f;
    [SerializeField] private float highestClamp = 200f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
    }
    private void LateUpdate()
    {
        HandleMovement();
        ClampPosition();
    }

    private void HandleMovement()
    {
        float input = InputHandler.Instance.CameraScroll;
        if(input != 0f)
        {
            transform.position += Vector3.up * input * moveSensitivity * Time.deltaTime;
        }
    }
    private void ClampPosition()
    {
        if(transform.position.y < lowestClamp)
        {
            transform.position = new Vector3(transform.position.x, lowestClamp, transform.position.z);
        }
        if(transform.position.y > highestClamp)
        {
            transform.position = new Vector3(transform.position.x, highestClamp, transform.position.z);
        }
    }
}
