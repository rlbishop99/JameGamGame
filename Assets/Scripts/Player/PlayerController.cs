using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour, IHasHealth
{
    public static PlayerController Instance { get; private set; }
    private Rigidbody rb;


    # region Movement
    [SerializeField] private float speedMax = 3f;
    private float speedCurrent;
    private float slowdownTimer;
    [SerializeField] private float jumpPower = 2f;
    [SerializeField] private float lookSpeed = 10f;
    private bool isMoving;
    private float canJump = 0.2f;
    private float resetJump = 0f;
    # endregion

    # region Managers
    [SerializeField] private GameInput gameInput;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject musicManager;
    # endregion

    # region Stats
    [SerializeField] private float healthMax = 60f;
    private float healthCurrent;
    public event EventHandler<IHasHealth.OnHealthChangedEventArgs> OnHealthChanged;
    private bool isDead;
    # endregion

    private Crank selectedCrank;

    [SerializeField] private CapsuleCollider bodyCollider;

    private void Awake()
    {
        if (Instance != null) {
            Debug.LogError("More than one player instance.");
        }
        Instance = this;

        rb = GetComponent<Rigidbody>();

        musicManager = GameObject.FindGameObjectWithTag("MusicManager");
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
        if (canJump <= 0) return;

        rb.AddForce(new Vector3(0f, jumpPower, 0f), ForceMode.Impulse);
        canJump = 0f;
        resetJump = 0f;
        musicManager.GetComponent<MusicManager>().SetAndPlaySound("Jump");
    }

    private void Update()
    {        
        canJump -= Time.deltaTime;

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
            isMoving = true;
            transform.position += (direction * speedCurrent) * Time.deltaTime;
            transform.forward = Vector3.Slerp(transform.forward, direction, Time.deltaTime * lookSpeed);
        } else {

            isMoving = false;

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
        if (Physics.CapsuleCast(transform.position, transform.position + Vector3.up *  bodyCollider.height, bodyCollider.radius, direction, out RaycastHit hitInfo, interactDistance)) {
            if (hitInfo.transform.TryGetComponent(out Crank crank)) {
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

    private void OnCollisionEnter(Collision obj)
    {

        switch (obj.gameObject.tag)
        {
            case "Enemy":
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

                GameObject effect = (GameObject)Instantiate(obj.gameObject.GetComponentInParent<enemy>().deathEffect, transform.position, Quaternion.identity);
                Destroy(effect, 3f);
                musicManager.GetComponent<MusicManager>().SetAndPlaySound("EnemyDeath");
                Destroy(obj.gameObject.transform.parent.gameObject);
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(new Vector3(0f, (jumpPower - 1), 0f), ForceMode.Impulse);
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

    private void OnTriggerStay() {
        resetJump += Time.deltaTime;

        if(resetJump >= .15f) {

            canJump = .2f;

        }
    }

    private void Die() {
        musicManager.GetComponent<MusicManager>().SetAndPlaySound("PlayerDeath");
        isDead = true;
    }

    public bool IsDead() {
        return isDead;
    }

    public void SetJump() {
        canJump = 0f;
    }
}