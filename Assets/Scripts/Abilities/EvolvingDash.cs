using UnityEngine;

public class EvolvingDash : BaseSpecialAbility
{
    public ParticleSystem explosionEffect; // For stage 1 (Explosive Dash)
    public ParticleSystem fireTrailEffect; // For stage 2 (Fire Dash)
    public ParticleSystem lightningEffect; // For stage 3 (Lightning Dash).

    bool isStarted = false;

    protected override void OnStart()
    {
        if (isStarted)
            return;
        Debug.Log("Evolving Dash Started");
        isStarted = true;
        // Determine the dash behavior based on the current level
        switch (Level)
        {
            case 1:
                ExplosiveDashStart();
                break;
            case 2:
                FireDashStart();
                break;
            case 3:
                LightningDashStart();
                break;
        }
    }

    protected override void OnUpdate()
    {
        // Dashing logic: keep moving forward until the duration ends
        if (spawner.isDashing)
        {
            spawner.dashSpeed = Speed;
            spawner.dashDamage = Damage;
            //transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }
    }

    protected override void OnAbilityEnd()
    {
        Debug.Log("Evolving Dash Ended");

        // Stop effects when dash ends
        if (fireTrailEffect.isPlaying)
            fireTrailEffect.Stop();
        if (lightningEffect.isPlaying)
            lightningEffect.Stop();
    }

    protected override void OnTriggerEffect(Collider other)
    {
        Debug.Log("Dash impact with " + other.gameObject.name);

        // Apply dash effects depending on the stage
        switch (Level)
        {
            case 1:
                ExplosiveDashImpact(other);
                break;
            case 2:
                FireDashImpact(other);
                break;
            case 3:
                LightningDashImpact(other);
                break;
        }
    }

    #region Stage Specific Logic

    // Stage 1: Explosive Dash Logi
    private void ExplosiveDashStart()
    {
        spawner.isDashing = true;
        Instantiate(explosionEffect, transform.position, explosionEffect.transform.rotation, gameObject.transform);
        Debug.Log("Explosive Dash started");
    }

    private void ExplosiveDashImpact(Collider other)
    {
        //if (explosionEffect != null)
        //{
        //    Debug.Log($"Dash Instantiate");
        //    Instantiate(explosionEffect, transform.position, Quaternion.identity, gameObject.transform);
        //}

        // Apply explosion damage to the enemy
        //other.gameObject.GetComponent<Move>().ApplyDamage(Damage);
        Debug.Log("Explosive Dash Impact");
    }

    // Stage 2: Fire Dash Logic
    private void FireDashStart()
    {
        spawner.isDashing = true;
        if (fireTrailEffect != null)
        {
            fireTrailEffect.gameObject.transform.parent = spawner.transform;
            fireTrailEffect.Play();
        }
        Debug.Log("Fire Dash started");
    }

    private void FireDashImpact(Collider other)
    {
        // Damage over time logic for enemies in the fire trail
        //other.gameObject.GetComponent<Move>().ApplyDamage(Damage * Time.deltaTime); // For example
        Debug.Log("Fire Dash Impact with damage over time");
    }

    // Stage 3: Lightning Dash Logic
    private void LightningDashStart()
    {
        spawner.isDashing = true;
        if (lightningEffect != null)
        {
            Instantiate(lightningEffect, transform.position, Quaternion.identity, gameObject.transform);
        }
        Debug.Log("Lightning Dash started");
    }

    private void LightningDashImpact(Collider other)
    {
        // Stun or damage enemies in a radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, EffectRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                //hitCollider.GetComponent<Move>().ApplyDamage(Damage);
                Debug.Log("Lightning Dash Impact and chain damage");
            }
        }
    }

    #endregion
}
