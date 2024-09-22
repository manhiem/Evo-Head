using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Move : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;

    Animator animator;

    public float playerSpeed = 6f;
    public float sprintSpeed = 10f;
    public float crouchSpeed = 3f;
    public float gravity = -9.8f;
    public float standingHeight = 3f;
    public float speedMultiplierInAir = 1.5f;

    public float minJumpHeight = 2.5f;
    public float maxJumpHeight = 6f;
    private float jumpHoldDuration = 0.2f;
    public float jumpTime = 1.5f;
    private bool isJumping = false;
    private bool jumpButtonHeld = false;
    public Transform camTransform;
    public Transform atkTransform;
    public bool isWalking = false;

    [Header("Health Params")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float timeBeforeHPRegenStarts = 3;
    [SerializeField] private float healthIncrementValue = 1;
    [SerializeField] private float healthTimeIncrement = 0.1f;
    private float currentHealth;

    [Header("Ability Params")]
    [SerializeField] private KeyCode AbilityTestKey;
    [SerializeField] private BaseSpecialAbility SpecialAbility;


    [HideInInspector] public float dashSpeed;
    [HideInInspector] public float dashDamage;
    [HideInInspector] public bool isDashing;



    void Start()
    {
        controller = GetComponent<CharacterController>();
        isJumping = false;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        MovePlayer();

        if (isGrounded())
        {
            if (Input.GetButtonDown("Jump"))
            {
                StartJump();
            }
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            HoldJump();
        }

        if (isDashing)
        {
            Debug.Log($"Dashing!! {dashSpeed}");
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 moveDirection = transform.TransformDirection(Vector3.forward) * dashSpeed;
            controller.Move(moveDirection * Time.deltaTime);
        }

        ApplyGravity();

        // Move the character controller based on velocity
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void MovePlayer()
    {
        float moveSpeed = GetMoveSpeed();
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.TransformDirection(new Vector3(horizontalInput, 0, verticalInput)) * moveSpeed;
        controller.Move(moveDirection * Time.deltaTime);

        if (moveDirection.magnitude >= 0.1f)  // Ensure there is actual input
        {
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                // Running state
                moveSpeed = sprintSpeed;
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);  // Ensure walking is false when running
            }
            else
            {
                // Walking state
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);  // Ensure running is false when walking
            }

            animator.SetBool("isIdle", false);  // Not idle while moving
        }

        else
        {
            // No movement input, so the player is idle
            animator.SetBool("isIdle", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }

        if (!isGrounded())
        {
            moveSpeed *= speedMultiplierInAir;
        }

        if(Input.GetKeyDown(AbilityTestKey))
        {
            InstantiateAbility(SpecialAbility);
        }
    }

    public void InstantiateAbility(BaseSpecialAbility ability)
    {
        BaseSpecialAbility abilityInstance = Instantiate(ability, atkTransform.position, ability.transform.rotation, gameObject.transform);
        abilityInstance.spawner = this;
    }
    float GetMoveSpeed()
    {
        return playerSpeed;
    }

    void ApplyGravity()
    {
        if (isGrounded())
        {
            playerVelocity.y = 0f;  // Reset vertical velocity when grounded
        }
        else
        {
            playerVelocity.y += gravity * Time.deltaTime;  // Apply gravity over time
        }

        // Apply gravity to the CharacterController
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void ApplyDamage(float dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
            KillPlayer();
    }

    private void KillPlayer()
    {
        currentHealth = 0;
        Destroy(gameObject);
        Debug.Log($"Player Dead!");
    }

    // void StandUp()
    // {
    //     isCrouching = false;
    //     controller.height = standingHeight;
    //     camTransform.localPosition = Vector3.zero;
    // }
    void StartJump()
    {
        Debug.Log("Jump started");
        isJumping = true;
        jumpTime = 0f;

        // Jump velocity calculation
        playerVelocity.y = Mathf.Sqrt(2f * minJumpHeight * gravity);  // Set initial jump velocity

        Debug.Log("Initial Jump Velocity: " + playerVelocity.y);
    }

    void HoldJump()
    {
        if (jumpTime < jumpHoldDuration)
        {
            jumpTime += Time.deltaTime;

            float heightMultiplier = Mathf.Clamp(jumpTime / jumpHoldDuration, 0f, 1f); // Clamps the time held between 0 and 1
            float targetJumpHeight = Mathf.Lerp(minJumpHeight, maxJumpHeight, heightMultiplier); // Linearly interpolate between min and max height

            playerVelocity.y = Mathf.Sqrt(2f * targetJumpHeight * gravity); // Update player velocity for higher jump
        }
        else
        {
            isJumping = false; // End the jump hold
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            if(isDashing)
            {
                // TODO: Deduct Enemy Health
                Debug.Log($"Enemy Hit! {dashDamage}");
            }
        }
    }

    bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.1f);
    }
}
