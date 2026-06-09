// InputManager.cs
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, IService
{
    private InputSystem_Actions _playerInput;
    
    public Vector2 MoveDirection { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool MenuPressed { get; private set; }
    public bool ShootPressed { get; private set; }
    public InputDevice CurrentDevice { get; private set; }
    
    public void Init()
    {
        _playerInput = new InputSystem_Actions();
        _playerInput.Player.Enable();

        _playerInput.Player.Move.performed += OnMove;
        _playerInput.Player.Move.canceled += OnMoveStop;
        _playerInput.Player.Jump.performed += OnJump;
        _playerInput.Player.Jump.canceled += OnJumpStop;
        _playerInput.Player.Menu.performed += OnMenu;
        _playerInput.Player.Shoot.performed += OnShoot;

        InputSystem.onActionChange += OnActionChange;
        
        Debug.Log("InputManager init called");
    }

    private void OnActionChange(object obj, InputActionChange change)
    {
        if (change != InputActionChange.ActionPerformed) return;
        if (obj is InputAction action && action.activeControl?.device != null)
            CurrentDevice = action.activeControl.device;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        MoveDirection = ctx.ReadValue<float>() == 0 
            ? Vector2.zero 
            : new Vector2(ctx.ReadValue<float>(), 0);
        CurrentDevice = ctx.control.device;
    }

    private void OnMoveStop(InputAction.CallbackContext ctx) => MoveDirection = Vector2.zero;

    private void OnJump(InputAction.CallbackContext ctx)
    {
        JumpPressed = true;
        CurrentDevice = ctx.control.device;
    }

    private void OnJumpStop(InputAction.CallbackContext ctx) => JumpPressed = false;

    private void OnMenu(InputAction.CallbackContext ctx)
    {
        MenuPressed = true;
        CurrentDevice = ctx.control.device;
    }

    private void OnShoot(InputAction.CallbackContext ctx)
    {
        ShootPressed = true;
        CurrentDevice = ctx.control.device;
    }

    public void ConsumeJump() => JumpPressed = false;
    public void ConsumeMenu() => MenuPressed = false;
    public void ConsumeShoot() => ShootPressed = false;

    private void OnDestroy()
    {
        _playerInput.Player.Move.performed -= OnMove;
        _playerInput.Player.Move.canceled -= OnMoveStop;
        _playerInput.Player.Jump.performed -= OnJump;
        _playerInput.Player.Jump.canceled -= OnJumpStop;
        _playerInput.Player.Menu.performed -= OnMenu;
        _playerInput.Player.Shoot.performed -= OnShoot;
        InputSystem.onActionChange -= OnActionChange;
        _playerInput.Dispose();
    }
}