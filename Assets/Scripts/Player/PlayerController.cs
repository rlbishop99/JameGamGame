    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour, IHasHealth
{
    public static PlayerController Instance { get; private set; }
    private Rigidbody rb;


    # region Movement
    [SerializeField] private float speedMax = 3f;
    private float speedCurrent;
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

    # region Stats
    [SerializeField] private float healthMax = 60f;
    private float healthCurrent;
    public event EventHandler<IHasHealth.OnHealthChangedEventArgs> OnHealthChanged;
    # endregion

    private Crank selectedCrank;

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

        speedCurrent = speedMax;
        healthCurrent = healthMax + GameManager.Instance.GetStartingTimer();
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (selectedCrank != null) {
            selectedCrank.CrankUp();
        }
    }

    private void GameInput_OnJumpAction(object sender, EventArgs e)
    {
        if (!isGrounded) return;

        rb.AddForce(new Vector3(0f, jumpPower, 0f), ForceMode.Impulse);
    }

    private void Update()
    {
        HandleHealth();
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
            transform.position += (direction * speedCurrent) * Time.deltaTime;
            transform.forward = Vector3.Slerp(transform.forward, direction, Time.deltaTime * lookSpeed);
        }

        if (slowdownTimer > 0) {
            slowdownTimer -= Time.deltaTime;
        } else {
            speedCurrent = speedMax;
        }
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVector();

        Vector3 direction = new Vector3(inputVector.x, 0f, inputVector.y);

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, direction, out RaycastHit raycastHit, interactDistance)) {
            if (raycastHit.transform.TryGetComponent(out Crank crank)) {
                selectedCrank = crank;
            } else {
                selectedCrank = null;
            }
        }
    }

    private void HandleHealth()
    {
        healthCurrent -= Time.deltaTime;
        if (healthCurrent <= 0) {
            Die();
        }

        OnHealthChanged?.Invoke(this, new IHasHealth.OnHealthChangedEventArgs {
            healthNormalized = healthCurrent/healthMax
        });
    }

    public void SlowDown(float factor = 0.5f, float durationSeconds = 5f)
    {
        speedCurrent = speedCurrent * factor;
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

    private void OnCollisionEnter(Collision obj)
    {
        switch (obj.gameObject.tag)
        {
            case "Enemy":
                healthCurrent -= obj.gameObject.GetComponent<enemy>().GetPlayerDamage();
                OnHealthChanged?.Invoke(this, new IHasHealth.OnHealthChangedEventArgs {
                    healthNormalized = healthCurrent/healthMax
                });
                if (healthCurrent <= 0) {
                    Die();
                }
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider obj)
    {
        switch (obj.gameObject.tag)
        {
            case "EnemyHead":
                Destroy(obj.gameObject.transform.parent.gameObject);
                rb.AddForce(new Vector3(0f, jumpPower, 0f), ForceMode.Impulse);
                healthCurrent += obj.gameObject.transform.parent.gameObject.GetComponent<enemy>().GetPlayerHeal();
                healthCurrent = Mathf.Min(healthCurrent, healthMax);
                OnHealthChanged?.Invoke(this, new IHasHealth.OnHealthChangedEventArgs {
                    healthNormalized = healthCurrent/healthMax
                });
                break;
            case "Ground":
                Die();
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

    private void Die() {
        Loader.Load(Loader.Scene.ProjectileScene);
    }
}