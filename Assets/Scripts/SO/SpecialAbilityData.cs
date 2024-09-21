using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "AbilityData")]
public class SpecialAbilityData : ScriptableObject
{
    public string AbilityName;
    public string Description;

    public AbilityFormula Damage;
    public AbilityFormula Range;
    public AbilityFormula Duration;
    public AbilityFormula AbilityCooldown;
    public AbilityFormula EffectRadius;
    public AbilityFormula Speed;
}
