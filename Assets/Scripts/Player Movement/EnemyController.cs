using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public enum MovementType { Crawl, Walk }
    public MovementType movementType;

    public float largeRadius = 20f;
    public float attackRange = 2f;
    public float moveSpeed = 2f;
    public float stoppingDistance = 1.5f; // Stopping distance before the enemy stops moving towards the player
    public float rotationSpeed = 5f;
    public int maxHealth = 100;
    public TextMeshProUGUI healthText;
    public GameObject healthBarUI;
    public Slider healthBar;
    public GameObject damageTextPrefab;

    private int currentHealth;
    private Transform player;
    private Animator animator;
    private bool isAttacking = false;
    private bool isLanding = false;
    private float groundCheckDistance = 1f;  // Adjust as needed for ground detection
    public LayerMask groundLayer;

    void Start()
    {
        currentHealth = maxHealth;
        //healthBar.maxValue = maxHealth;
        //healthBar.value = currentHealth;
        healthText.text = $"Health: " + currentHealth.ToString("00");

        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        CheckForLanding();
    }

    void Update()
    {
        if (!isLanding)
        {
            if (Vector3.Distance(transform.position, player.position) <= largeRadius)
            {
                FollowPlayer();

                if (Vector3.Distance(transform.position, player.position) <= attackRange && !isAttacking)
                {
                    StartCoroutine(AttackPlayer());
                }
            }
            else
            {
                animator.SetTrigger("Idle");
            }

            //healthBar.value = currentHealth;
            healthText.text = $"Health: " + currentHealth.ToString("00");
        }
    }

    void CheckForLanding()
    {
        // Raycast to check distance from ground
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundLayer))
        {
            // If close to the ground, no need to play landing animation
            StartIdle();
        }
        else
        {
            // If not close to the ground, play landing animation
            isLanding = true;
            animator.Play("ZombieLanding");
            StartCoroutine(StartAnimationsAfterLanding());
        }
    }

    IEnumerator StartAnimationsAfterLanding()
    {
        // Adjust based on the landing animation length
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isLanding = false;
        StartIdle();
    }

    void StartIdle()
    {
        animator.SetTrigger("Idle");
    }

    void FollowPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > stoppingDistance)
        {
            // Move towards the player
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            if (movementType == MovementType.Crawl)
            {
                animator.SetTrigger("Crawl");
                transform.position += transform.forward * (moveSpeed * 0.5f) * Time.deltaTime;
            }
            else
            {
                animator.SetTrigger("Run");
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
        }
        else
        {
            // Stop moving and prepare for attack
            animator.SetTrigger("Idle");
        }
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;

        // Play attack animation based on movement type
        if (movementType == MovementType.Crawl)
        {
            animator.SetTrigger("Bite");
        }
        else
        {
            animator.SetTrigger("WalkBite"); // This is the new walking attack animation
        }

        yield return new WaitForSeconds(1f); // Attack animation time
        // Apply damage to the player here

        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Display floating damage text
        GameObject damageText = Instantiate(damageTextPrefab, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        damageText.GetComponent<Text>().text = damage.ToString();
        Destroy(damageText, 1f);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Play death animation or destroy the enemy object
        animator.SetTrigger("Die");
        Destroy(gameObject, 3f); // Delay for death animation
    }
}
