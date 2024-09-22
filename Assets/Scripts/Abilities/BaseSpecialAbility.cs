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

    float castTime;
    public Move spawner;

    // Use this key format for saving/loading PlayerPrefs
    private string abilityLevelKey = "AbilityLevel_";

    private void Start()
    {
        // Retrieve saved level from PlayerPrefs or set default (1 if none exists)
        //Level = PlayerPrefs.GetInt(abilityLevelKey + gameObject.name, 1);
        castTime = Time.time;
        OnStart();
    }

    private void Update()
    {
        if (Time.time - castTime < Duration)
        {
            Debug.Log($"Speed: {Speed}");
            Debug.Log($"Damage: {Damage}");
            Debug.Log($"Duration: {Duration}");
            Debug.Log($"Range: {Range}");
            Debug.Log($"AbilityCooldown: {AbilityCooldown}");
            Debug.Log($"EffectRadius: {EffectRadius}");

            OnUpdate();
        }
        else
        {
            spawner.isDashing = false;
            OnAbilityEnd();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Deduct health
            //other.gameObject.GetComponent<Move>().ApplyDamage(Damage);
            Debug.Log($"Health Minus Minus");
            OnTriggerEffect(other);
        }
    }

    // Method to update and save the ability level
    public void UpdateAbilityLevel(int newLevel)
    {
        Level = newLevel;

        // Save the updated level in PlayerPrefs
        PlayerPrefs.SetInt(abilityLevelKey + gameObject.name, Level);
        PlayerPrefs.Save();
    }

    // Abstract methods for derived classes to implement their specific behavior
    protected abstract void OnStart();
    protected abstract void OnUpdate();
    protected abstract void OnAbilityEnd();
    protected abstract void OnTriggerEffect(Collider other);
}
