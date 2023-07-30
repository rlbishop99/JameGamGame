using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    private Rigidbody rb;

    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpPower = 3f;
    [SerializeField] private float lookSpeed = 10f;
    [SerializeField] private GameInput gameInput;

    private bool isMoving;
    private bool isGrounded;


    private void Awake()
    {
        if (Instance != null) {
            Debug.LogError("More than one player instance.");
        }
        Instance = this;

        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        gameInput.OnJumpAction += GameInput_OnJumpAction;
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        Debug.Log("Interacted");
    }

    private void GameInput_OnJumpAction(object sender, EventArgs e)
    {
        if (!isGrounded) return;

        rb.AddForce(new Vector3(0f, jumpPower, 0f), ForceMode.Impulse);
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVector();
        Vector3 direction = new Vector3(inputVector.x, 0f, inputVector.y);
        
        if (direction != Vector3.zero)
        {
            transform.position += (direction * speed) * Time.deltaTime;
            transform.forward = Vector3.Slerp(transform.forward, direction, Time.deltaTime * lookSpeed);
        }
    }

    private void HandleInteractions() {}

    private void OnCollisionExit()
    {
        isGrounded = false;
    }
    private void OnCollisionStay()
    {
        isGrounded = true;
    }

    private void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag("Enemy")) 
        {
            Destroy(obj.gameObject);
        }
    }
}


