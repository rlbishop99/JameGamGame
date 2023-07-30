using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Look();
        Move();
        characterController.Move(direction * speed * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        direction = new Vector3(input.x, 0f, input.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("TO THE MOOOOON");
    }

    public void Look()
    {
        if (input.sqrMagnitude == 0) return;

        var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        targetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref velocity, smoothTime);
        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
    }
}
