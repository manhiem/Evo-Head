using UnityEngine;

public class EvolvingMagnet : BaseSpecialAbility
{
    public float magnetRange = 5f; // Magnetize attraction range
    public float repelForce = 500f; // Magnetic Wave repel force
    public ParticleSystem blackHoleEffect; // Particle effect for the Black Hole
    public ParticleSystem repelEffect;
    public ParticleSystem attractEffect;

    private bool isActive = false;

    protected override void OnStart()
    {
        Debug.Log("Evolving Magnet Started");

        // Determine the magnet behavior based on the current level
        switch (Level)
        {
            case 1:
                MagnetizeStart();
                break;
            case 2:
                MagneticWaveStart();
                break;
            case 3:
                BlackHoleStart();
                break;
        }
    }

    protected override void OnUpdate()
    {
        // Execute ability logic during the active phase
        if (isActive)
        {
            switch (Level)
            {
                case 1:
                    MagnetizeEffect();
                    break;
                case 2:
                    MagneticWaveEffect();
                    break;
                case 3:
                    BlackHoleEffect();
                    break;
            }
        }
    }

    protected override void OnAbilityEnd()
    {
        Debug.Log("Evolving Magnet Ended");

        if (blackHoleEffect != null && blackHoleEffect.isPlaying)
        {
            blackHoleEffect.Stop();
        }

        attractEffect.Stop();
        repelEffect.Stop();
        // Reset active state
        isActive = false;
    }

    protected override void OnTriggerEffect(Collider other)
    {
        // No specific impact logic needed for Magnetize as it doesn't involve direct collisions
        Debug.Log("Magnet effect triggered on " + other.gameObject.name);
    }

    #region Stage Specific Logic

    // Stage 1: Magnetize - Attracts nearby items and ammo
    private void MagnetizeStart()
    {
        isActive = true;
        attractEffect.Play();
        Debug.Log("Magnetize started");
    }

    private void MagnetizeEffect()
    {
        Collider[] items = Physics.OverlapSphere(transform.position, magnetRange);
        foreach (var item in items)
        {
            if (item.CompareTag("Item"))
            {
                Vector3 direction = transform.position - item.transform.position;
                item.transform.position = Vector3.MoveTowards(item.transform.position, transform.position, Speed * Time.deltaTime);
                Debug.Log("Attracting item: " + item.name);
            }
        }
    }

    // Stage 2: Magnetic Wave - Repels enemies while pulling in items
    private void MagneticWaveStart()
    {
        isActive = true;
        repelEffect.Play();
        Debug.Log("Magnetic Wave started");
    }

    private void MagneticWaveEffect()
    {
        Collider[] objects = Physics.OverlapSphere(transform.position, magnetRange);
        foreach (var obj in objects)
        {
            if (obj.CompareTag("Item"))
            {
                // Pull items toward the player
                obj.transform.position = Vector3.MoveTowards(obj.transform.position, transform.position, Speed * Time.deltaTime);
                Debug.Log("Attracting item: " + obj.name);
            }
            else if (obj.CompareTag("Enemy"))
            {
                // Repel enemies away from the player
                Rigidbody enemyRb = obj.GetComponent<Rigidbody>();
                if (enemyRb != null)
                {
                    Vector3 repelDirection = obj.transform.position - transform.position;
                    enemyRb.AddForce(repelDirection.normalized * repelForce);
                    Debug.Log("Repelling enemy: " + obj.name);
                }
            }
        }
    }

    // Stage 3: Black Hole - Pulls in enemies and items, damages them over time, and then explodes
    private void BlackHoleStart()
    {
        isActive = true;
        if (blackHoleEffect != null)
        {
            blackHoleEffect.Play();
        }
        Debug.Log("Black Hole started");
    }

    private void BlackHoleEffect()
    {
        Collider[] objects = Physics.OverlapSphere(transform.position, EffectRadius);
        foreach (var obj in objects)
        {
            if (obj.CompareTag("Item") || obj.CompareTag("Enemy"))
            {
                // Pull enemies and items toward the black hole
                obj.transform.position = Vector3.MoveTowards(obj.transform.position, transform.position, Speed * Time.deltaTime);

                if (obj.CompareTag("Enemy"))
                {
                    // Deal damage to enemies over time
                    obj.GetComponent<Move>().ApplyDamage(Damage * Time.deltaTime);
                    Debug.Log("Damaging enemy: " + obj.name);
                }
            }
        }
    }

    #endregion
}
