using System;
using Unity.VisualScripting;
using UnityEngine;

public class Move : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;

    public float playerSpeed = 6f;
    public float sprintSpeed = 10f;
    public float crouchSpeed = 3f;
    public float gravity = 9.8f;
    public float standingHeight = 2f;
    public float speedMultiplierInAir = 1.5f;

    public float minJumpHeight = 2.5f;
    public float maxJumpHeight = 6f;
    private float jumpHoldDuration = 0.2f;
    public float jumpTime = 1.5f;
    private bool isJumping = false;
    private bool jumpButtonHeld = false;
    public Transform camTransform;
    public Transform atkTransform;

    [Header("Health Params")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float timeBeforeHPRegenStarts = 3;
    [SerializeField] private float healthIncrementValue = 1;
    [SerializeField] private float healthTimeIncrement = 0.1f;
    private float currentHealth;

    [Header("Ability Params")]
    [SerializeField] private KeyCode AbilityTestKey;
    [SerializeField] private BaseSpecialAbility SpecialAbility;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        isJumping = false;
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

        ApplyGravity();

        // Move the character controller based on velocity
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void MovePlayer()
    {
        float moveSpeed = GetMoveSpeed();
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = sprintSpeed;
        }

        if (!isGrounded())
        {
            moveSpeed *= speedMultiplierInAir;
        }

        Vector3 moveDirection = transform.TransformDirection(new Vector3(horizontalInput, 0, verticalInput)) * moveSpeed;
        controller.Move(moveDirection * Time.deltaTime);
    }

    public void InstantiateAbility(BaseSpecialAbility ability)
    {
        Instantiate(ability, atkTransform.position, ability.transform.rotation);
    }
    float GetMoveSpeed()
    {
        return playerSpeed;
    }

    void ApplyGravity()
    {
        playerVelocity.y += gravity * Time.deltaTime;
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

    bool isGrounded()
    {
        return controller.isGrounded;
    }
}
