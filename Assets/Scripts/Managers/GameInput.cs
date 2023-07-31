using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";
    public static GameInput Instance { get; private set; }

    public event EventHandler OnJumpAction;
    public event EventHandler OnInteractAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;

    public enum Binding {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        Pause,
        Gamepad_Interact,
        Gamepad_Pause
    }

    private PlayerInput playerInput;
    
    private void Awake()
    {
        Instance = this;

        playerInput = new PlayerInput();
        playerInput.PlayerControls.Enable();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS)) {
            playerInput.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        playerInput.PlayerControls.Jump.performed += Jump_performed;
        playerInput.PlayerControls.Interact.performed += Interact_performed;
        playerInput.PlayerControls.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        playerInput.PlayerControls.Jump.performed -= Jump_performed;
        playerInput.PlayerControls.Interact.performed -= Interact_performed;
        playerInput.PlayerControls.Pause.performed -= Pause_performed;

        playerInput.Dispose();
    }

    private void Jump_performed(InputAction.CallbackContext context)
    {
        OnJumpAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(InputAction.CallbackContext context)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void Pause_performed(InputAction.CallbackContext context)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVector(bool normalized = true)
    {
        Vector2 inputVector = playerInput.PlayerControls.Move.ReadValue<Vector2>();
        return normalized ? inputVector.normalized : inputVector;
    }

    public string GetBindingText(Binding binding) {
        switch (binding) {
            default:
            case Binding.Move_Up:
                return playerInput.PlayerControls.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInput.PlayerControls.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInput.PlayerControls.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInput.PlayerControls.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInput.PlayerControls.Interact.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInput.PlayerControls.Pause.bindings[0].ToDisplayString();
            case Binding.Gamepad_Interact:
                return playerInput.PlayerControls.Interact.bindings[1].ToDisplayString();
            case Binding.Gamepad_Pause:
                return playerInput.PlayerControls.Pause.bindings[1].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound) {
        playerInput.PlayerControls.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding) {
            default:
            case Binding.Move_Up:
                inputAction = playerInput.PlayerControls.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInput.PlayerControls.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInput.PlayerControls.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInput.PlayerControls.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInput.PlayerControls.Interact;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInput.PlayerControls.Pause;
                bindingIndex = 0;
                break;
            case Binding.Gamepad_Interact:
                inputAction = playerInput.PlayerControls.Interact;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_Pause:
                inputAction = playerInput.PlayerControls.Pause;
                bindingIndex = 1;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback => {
                callback.Dispose();
                playerInput.PlayerControls.Enable();
                onActionRebound();

                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInput.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            })
            .Start();
    }

    public void DisableMovement()
    {
        playerInput.PlayerControls.Move.Disable();
        playerInput.PlayerControls.Jump.Disable();
    }

    public void EnableMovement()
    {
        playerInput.PlayerControls.Move.Enable();
        playerInput.PlayerControls.Jump.Enable();
    }
}
