using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpecialAbility : MonoBehaviour
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

    float castTime;

    private void Start()
    {
        castTime = Time.time;
    }

    private void Update()
    {
        if(Time.time - castTime < Duration)
        {
            Debug.Log($"Speed: {Speed}");
            Debug.Log($"Damage: {Damage}");
            Debug.Log($"Duration: {Duration}");
            Debug.Log($"Range: {Range}");
            Debug.Log($"AbilityCooldown: {AbilityCooldown}");
            Debug.Log($"EffectRadius: {EffectRadius}");
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            // Deduct health
            other.gameObject.GetComponent<Move>().ApplyDamage(Damage);
            Debug.Log($"Health Minus Minus");
        }
    }
}
