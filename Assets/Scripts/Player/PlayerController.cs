using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    private Rigidbody rb;


    # region Movement
    [SerializeField] private float baseSpeed = 3f;
    private float currentSpeed;
    private float slowdownTimer;
    [SerializeField] private float jumpPower = 3f;
    [SerializeField] private float lookSpeed = 10f;
    private bool isMoving;
    private bool isGrounded;
    # endregion

    # region Managers
    [SerializeField] private GameInput gameInput;
    [SerializeField] private GameManager gameManager;
    # endregion


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

        currentSpeed = baseSpeed;
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
            transform.position += (direction * currentSpeed) * Time.deltaTime;
            transform.forward = Vector3.Slerp(transform.forward, direction, Time.deltaTime * lookSpeed);
        }

        if (slowdownTimer > 0) {
            slowdownTimer -= Time.deltaTime;
        } else {
            currentSpeed = baseSpeed;
        }
    }

    private void HandleInteractions() {}

    public void SlowDown(float factor = 0.5f, float durationSeconds = 5f)
    {
        currentSpeed = currentSpeed * factor;
        slowdownTimer += durationSeconds;
    }

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
        switch (obj.gameObject.tag)
        {
            case "EnemyHead":
                Destroy(obj.gameObject.transform.parent.gameObject);
                rb.AddForce(new Vector3(0f, jumpPower, 0f), ForceMode.Impulse);
                break;
            case "Ground":
                Loader.Load(Loader.Scene.SampleScene);
                break;
            case "Gear":
                gameObject.transform.SetParent(obj.transform, true);
                break;
        }
    }

    private void OnTriggerExit(Collider obj) {
        switch (obj.gameObject.tag)
        {
            case "Gear":
                gameObject.transform.parent = null;
                break;
        }
    }
}


