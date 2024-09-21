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
    public float gravity = -9.8f;
    public float standingHeight = 2f;
    public float speedMultiplierInAir = 1.5f;

    public float minJumpHeight = 2.5f;
    public float maxJumpHeight = 6f;
    private float jumpHoldTime = 0f;
    public float maxJumpHoldDuration = 1.5f;
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
        ApplyGravity();

        if (isGrounded() && Input.GetButtonDown("Jump"))
        {
            StartJump();
        }
        else if (isJumping && Input.GetButton("Jump") && jumpButtonHeld)
        {
            HoldJump();
        }

        if (Input.GetButtonUp("Jump"))
        {
            jumpButtonHeld = false;
        }

        if (isGrounded())
        {
            playerVelocity.y = 0f;
            isJumping = false;
        }
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
        isJumping = true;
        jumpButtonHeld = true;
        jumpHoldTime = 0f;
        playerVelocity.y = Mathf.Sqrt(minJumpHeight * -2f * gravity);
    }

    void HoldJump()
    {
        if (jumpHoldTime < maxJumpHoldDuration)
        {
            jumpHoldTime += Time.deltaTime;
            float holdPercentage = Mathf.Clamp(jumpHoldTime / maxJumpHoldDuration, 0f, 1f);
            float targetJumpHeight = Mathf.Lerp(minJumpHeight, maxJumpHeight, holdPercentage);
            playerVelocity.y = Mathf.Sqrt(targetJumpHeight * -2f * gravity);
        }
    }
    bool isGrounded()
    {
        return controller.isGrounded;
    }
}
