using System;
using Unity.VisualScripting;
using UnityEngine;

public class Move : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVel;

    public float playerSpeed = 6f;
    public float sprintSpeed = 10f;
    public float crouchSpeed = 3f;
    public float gravity = -9.8f;
    public float standingHeight = 2f;
    public float crouchHeight = 1f;
    private bool isCrouching;
    public float speedMulti = 1.5f;
    public float jumpHeight = 2.5f;
    public float maxJumpHeight = 6f;
    private float currentJumpHeight;
    private float jumpHoldTime = 0f;
    public float maxJumpHoldDur = 1.5f;
    private bool isJumping;

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
        // camTransform = Camera.main.transform;
        isCrouching = false;
        isJumping = false;

        currentHealth = maxHealth;
    }

    void Update()
    {
        MovePlayer();
        ApplyGravity();

        if (isGrounded() && Input.GetButtonDown("Jump"))
        {
            StartJump();
        }
        else if (isJumping && Input.GetButton("Jump"))
        {
            HoldJump();
        }
        else if (Input.GetButtonUp("Jump"))
        {
            EndJump();
        }
    }

    void MovePlayer()
    {
        float moveSpeed = GetMoveSpeed();
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            moveSpeed = sprintSpeed;
        }
        else if (Input.GetKey(KeyCode.C))
        {
            Crouch();
        }
        // else
        // {
        //    StandUp();
        // }
        if (!isGrounded())
        {
            moveSpeed *= speedMulti;
        }
        Vector3 moveDirection = transform.TransformDirection(new Vector3(horizontalInput, 0, verticalInput)) * moveSpeed;
        controller.Move(moveDirection * Time.deltaTime);
    }

    public void InstantiateAbility(BaseSpecialAbility ability)
    {
        Instantiate(ability, atkTransform.position, ability.transform.rotation);
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

    float GetMoveSpeed()
    {
        return isCrouching ? crouchSpeed : playerSpeed;
    }

    void Crouch()
    {
        isCrouching = true;
        controller.height = crouchHeight;
        camTransform.localPosition = new Vector3(camTransform.position.x, crouchHeight, camTransform.position.z);
    }

    // void StandUp()
    // {
    //     isCrouching = false;
    //     controller.height = standingHeight;
    //     camTransform.localPosition = Vector3.zero;
    // }

    void ApplyGravity()
    {
        playerVel.y += gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);
    }

    void StartJump()
    {
        isJumping = true;
        jumpHoldTime = 0f;
        currentJumpHeight = jumpHeight;
    }
    void HoldJump()
    {
        jumpHoldTime += Time.deltaTime;
        float holdPer = Mathf.Clamp(jumpHoldTime / maxJumpHoldDur, 0f, 1f);
        currentJumpHeight = Mathf.Lerp(jumpHeight, maxJumpHeight, holdPer);
    }
    void EndJump()
    {
        isJumping = false;
        playerVel.y = Mathf.Sqrt(currentJumpHeight * -2 * gravity);
    }

    bool isGrounded()
    {
        return controller.isGrounded;
    }
}
