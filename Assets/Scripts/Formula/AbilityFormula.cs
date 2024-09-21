using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Formulaes/Formula1")]
public abstract class AbilityFormula : ScriptableObject
{
    public float baseValue;
    public int multiplier;

    public abstract float Evaluate(int level);
}
