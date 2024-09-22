using UnityEngine;

public class EvolvingJump : BaseSpecialAbility
{
    public ParticleSystem explosionEffect; // Particle effect for the Meteor Jump explosion
    public ParticleSystem shockwaveEffect; // Particle effect for the Shockwave Jump

    private bool isJumping = false;
    private Rigidbody rb;

    private void Awake()
    {

    }

    protected override void OnStart()
    {
        Debug.Log("Evolving Jump Started");
        rb = spawner.GetComponent<Rigidbody>();
        // Determine the jump behavior based on the current level
        switch (Level)
        {
            case 1:
                JellyJumpStart();
                break;
            case 2:
                MeteorJumpStart();
                break;
            case 3:
                ShockwaveJumpStart();
                break;
        }
    }

    protected override void OnUpdate()
    {
        // Apply jump behavior
        if (isJumping && rb.velocity.y <= 0)
        {
            switch (Level)
            {
                case 1:
                    JellyJumpImpact();
                    break;
                case 2:
                    MeteorJumpImpact();
                    break;
                case 3:
                    ShockwaveJumpImpact();
                    break;
            }
        }
    }

    protected override void OnAbilityEnd()
    {
        Debug.Log("Evolving Jump Ended");
        isJumping = false;
    }

    protected override void OnTriggerEffect(Collider other)
    {
        Debug.Log("Evolving Jump Impact with " + other.gameObject.name);
    }

    #region Stage Specific Logic

    // Stage 1: Jelly Jump - Jump high and bounce around, squishing enemies
    private void JellyJumpStart()
    {
        isJumping = true;
        rb.AddForce(Vector3.up * Speed, ForceMode.Impulse);
        Debug.Log("Jelly Jump started, bouncing...");
    }

    private void JellyJumpImpact()
    {
        // On impact, apply squishing logic to enemies
        Collider[] enemies = Physics.OverlapSphere(transform.position, EffectRadius);
        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<Move>().ApplyDamage(Damage);
                Debug.Log("Jelly Jump squished enemy: " + enemy.gameObject.name);
            }
        }
    }

    // Stage 2: Meteor Jump - Create a small explosion on landing
    private void MeteorJumpStart()
    {
        isJumping = true;
        rb.AddForce(Vector3.up * Speed, ForceMode.Impulse);
        Debug.Log("Meteor Jump started, prepare for explosion...");
    }

    private void MeteorJumpImpact()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        Collider[] enemies = Physics.OverlapSphere(transform.position, EffectRadius);
        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<Move>().ApplyDamage(Damage);
                Debug.Log("Meteor Jump explosion damaged enemy: " + enemy.gameObject.name);
            }
        }
    }

    // Stage 3: Shockwave Jump - Create a powerful shockwave on landing
    private void ShockwaveJumpStart()
    {
        isJumping = true;
        rb.AddForce(Vector3.up * Speed, ForceMode.Impulse);
        Debug.Log("Shockwave Jump started, prepare for shockwave...");
    }

    private void ShockwaveJumpImpact()
    {
        if (shockwaveEffect != null)
        {
            Instantiate(shockwaveEffect, transform.position, Quaternion.identity);
        }
        Collider[] enemies = Physics.OverlapSphere(transform.position, EffectRadius);
        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<Rigidbody>().AddExplosionForce(Damage, transform.position, EffectRadius, 1.0f, ForceMode.Impulse);
                Debug.Log("Shockwave Jump knocked back enemy: " + enemy.gameObject.name);
            }
        }
    }

    #endregion
}
