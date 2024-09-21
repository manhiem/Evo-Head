using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSpecialAbility : MonoBehaviour
{
    public SpecialAbilityData data;

    #region Data Getters

    public int Level = 1;
    public float Damage => data.Damage.Evaluate(Level);
    public float Range => data.Range.Evaluate(Level);
    public float Duration => data.Duration.Evaluate(Level);
    public float AbilityCooldown => data.AbilityCooldown.Evaluate(Level);
    public float EffectRadius => data.EffectRadius.Evaluate(Level);
    public float Speed => data.Speed.Evaluate(Level);

    #endregion

    private void Start()
    {
        Destroy(gameObject, Duration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            // Deduct health
            Debug.Log($"Health Minus Minus");
        }
    }
}
