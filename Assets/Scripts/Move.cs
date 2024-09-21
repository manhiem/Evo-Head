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
    public float jumpHeight = 2f;
    private bool isCrouching;
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
        currentHealth = maxHealth;
    }

    void Update()
    {
        MovePlayer();
        ApplyGravity();

        if (isGrounded() && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if(Input.GetKeyDown(AbilityTestKey))
        {
            Instantiate(SpecialAbility, atkTransform.position, SpecialAbility.transform.rotation);
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

        Vector3 moveDirection = transform.TransformDirection(new Vector3(horizontalInput, 0, verticalInput)) * moveSpeed;
        controller.Move(moveDirection * Time.deltaTime);
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

    void Jump()
    {
        playerVel.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }

    bool isGrounded()
    {
        return controller.isGrounded;
    }

}