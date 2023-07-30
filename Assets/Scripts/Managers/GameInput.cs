using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnJumpAction;
    public event EventHandler OnInteractAction;
    public event EventHandler OnPauseAction;

    private PlayerInput playerInput;
    
    private void Awake()
    {
        Instance = this;

        playerInput = new PlayerInput();
        playerInput.PlayerControls.Enable();

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
}
