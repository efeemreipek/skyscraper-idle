using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : Singleton<InputHandler>
{
    [SerializeField] private InputActionAsset inputActionAsset;

    private InputActionMap actionMapGame;

    private InputAction actionMousePosition;
    private InputAction actionMouseLeftClick;

    private Vector2 mousePosition;
    private bool mouseLeftClickPressed, mouseLeftClickHold, mouseLeftClickReleased;

    public Vector2 MousePosition => mousePosition;
    public bool MouseLeftClickPressed => mouseLeftClickPressed;
    public bool MouseLeftClickHold => mouseLeftClickHold;
    public bool MouseLeftClickReleased => mouseLeftClickReleased;

    private void OnEnable()
    {
        if(inputActionAsset == null) inputActionAsset = InputSystem.actions;

        InitializeActionMaps();
        InitializeInputActions();

        EnableActionMaps();
        SubscribeToInputEvents();
    }
    private void OnDisable()
    {
        UnsubscribeFromInputEvents();
        DisableActionMaps();
    }
    private void InitializeActionMaps()
    {
        actionMapGame = inputActionAsset.FindActionMap("Game");
    }
    private void EnableActionMaps()
    {
        actionMapGame.Enable();
    }
    private void DisableActionMaps()
    {
        actionMapGame.Disable();
    }
    private void InitializeInputActions()
    {
        actionMousePosition = InputSystem.actions.FindAction("Mouse Position");
        actionMouseLeftClick = InputSystem.actions.FindAction("Mouse Left Click");
    }
    private void SubscribeToInputEvents()
    {
        actionMousePosition.performed += MousePosition_Performed;
        actionMousePosition.canceled += MousePosition_Canceled;

        actionMouseLeftClick.performed += MouseLeftClick_Performed;
        actionMouseLeftClick.canceled += MouseLeftClick_Canceled;
    }
    private void UnsubscribeFromInputEvents()
    {
        actionMousePosition.performed -= MousePosition_Performed;
        actionMousePosition.canceled -= MousePosition_Canceled;

        actionMouseLeftClick.performed -= MouseLeftClick_Performed;
        actionMouseLeftClick.canceled -= MouseLeftClick_Canceled;
    }

    private void LateUpdate()
    {
        mouseLeftClickPressed = false;
        mouseLeftClickReleased = false;
    }

    private void MousePosition_Performed(InputAction.CallbackContext obj) => mousePosition = obj.ReadValue<Vector2>();
    private void MousePosition_Canceled(InputAction.CallbackContext obj) => mousePosition = obj.ReadValue<Vector2>();
    private void MouseLeftClick_Performed(InputAction.CallbackContext obj)
    {
        mouseLeftClickPressed = true;
        mouseLeftClickHold = true;
    }
    private void MouseLeftClick_Canceled(InputAction.CallbackContext obj)
    {
        mouseLeftClickHold = false;
        mouseLeftClickReleased = true;
    }

    public void ResetInputStates()
    {
        mouseLeftClickPressed = false;
        mouseLeftClickHold = false;
    }
}
