using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityFormula : ScriptableObject
{
    public float baseValue;
    public float multiplier;

    public abstract float Evaluate(int level);
}
