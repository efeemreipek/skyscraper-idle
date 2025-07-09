using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputHandler : Singleton<InputHandler>
{
    [SerializeField] private InputActionAsset inputActionAsset;

    private InputActionMap actionMapGame;
    private InputActionMap actionMapCamera;

    private InputAction actionMousePosition;
    private InputAction actionMouseLeftClick;
    private InputAction actionPause;
    private InputAction actionCameraScroll;

    private Vector2 mousePosition;
    private bool mouseLeftClickPressed, mouseLeftClickHold, mouseLeftClickReleased;
    private bool pausePressed, pauseReleased;
    private float cameraScroll;

    public event Action OnMouseLeftClickPressed;
    public event Action OnPausePressed;

    public bool IsMouseOverUI => EventSystem.current.IsPointerOverGameObject();
    public Vector2 MousePosition => mousePosition;
    public bool MouseLeftClickPressed => mouseLeftClickPressed;
    public bool MouseLeftClickHold => mouseLeftClickHold;
    public bool MouseLeftClickReleased => mouseLeftClickReleased;
    public float CameraScroll => cameraScroll;
    public bool PausePressed => pausePressed;

    private void OnEnable()
    {
        if(inputActionAsset == null) inputActionAsset = InputSystem.actions;

        InitializeActionMaps();
        InitializeInputActions();

        EnableActions();
        SubscribeToInputEvents();
    }
    private void OnDisable()
    {
        UnsubscribeFromInputEvents();
        DisableActions();
    }
    private void InitializeActionMaps()
    {
        actionMapGame = inputActionAsset.FindActionMap("Game");
        actionMapCamera = inputActionAsset.FindActionMap("Camera");
    }
    private void EnableActions()
    {
        actionMousePosition.Enable();
        actionMouseLeftClick.Enable();
        actionPause.Enable();
        actionCameraScroll.Enable();
    }
    private void DisableActions()
    {
        actionMousePosition.Disable();
        actionMouseLeftClick.Disable();

        actionCameraScroll.Disable();
    }
    private void InitializeInputActions()
    {
        actionMousePosition = actionMapGame.FindAction("Mouse Position");
        actionMouseLeftClick = actionMapGame.FindAction("Mouse Left Click");
        actionPause = actionMapGame.FindAction("Pause");

        actionCameraScroll = actionMapCamera.FindAction("Camera Scroll");
    }
    private void SubscribeToInputEvents()
    {
        actionMousePosition.performed += MousePosition_Performed;
        actionMousePosition.canceled += MousePosition_Canceled;

        actionMouseLeftClick.performed += MouseLeftClick_Performed;
        actionMouseLeftClick.canceled += MouseLeftClick_Canceled;

        actionPause.performed += Pause_Performed;
        actionPause.canceled += Pause_Canceled;

        actionCameraScroll.performed += CameraScroll_Performed;
        actionCameraScroll.canceled += CameraScroll_Canceled;
    }
    private void UnsubscribeFromInputEvents()
    {
        actionMousePosition.performed -= MousePosition_Performed;
        actionMousePosition.canceled -= MousePosition_Canceled;

        actionMouseLeftClick.performed -= MouseLeftClick_Performed;
        actionMouseLeftClick.canceled -= MouseLeftClick_Canceled;

        actionPause.performed -= Pause_Performed;
        actionPause.canceled -= Pause_Canceled;

        actionCameraScroll.performed -= CameraScroll_Performed;
        actionCameraScroll.canceled -= CameraScroll_Canceled;
    }

    private void Update()
    {
        if(IsMouseOverUI)
        {
            DisableActions();
        }
        else
        {
            EnableActions();
        }
    }
    private void LateUpdate()
    {
        mouseLeftClickPressed = false;
        mouseLeftClickReleased = false;

        pausePressed = false;
        pauseReleased = false;
    }

    private void MousePosition_Performed(InputAction.CallbackContext obj) => mousePosition = obj.ReadValue<Vector2>();
    private void MousePosition_Canceled(InputAction.CallbackContext obj) => mousePosition = obj.ReadValue<Vector2>();
    private void MouseLeftClick_Performed(InputAction.CallbackContext obj)
    {
        mouseLeftClickPressed = true;
        OnMouseLeftClickPressed?.Invoke();
        mouseLeftClickHold = true;
    }
    private void MouseLeftClick_Canceled(InputAction.CallbackContext obj)
    {
        mouseLeftClickHold = false;
        mouseLeftClickReleased = true;
    }
    private void Pause_Performed(InputAction.CallbackContext obj)
    {
        pausePressed = true;
        OnPausePressed?.Invoke();
    }
    private void Pause_Canceled(InputAction.CallbackContext obj)
    {
        pauseReleased = true;
    }
    private void CameraScroll_Performed(InputAction.CallbackContext obj) => cameraScroll = obj.ReadValue<float>();
    private void CameraScroll_Canceled(InputAction.CallbackContext obj) => cameraScroll = 0f;
}
