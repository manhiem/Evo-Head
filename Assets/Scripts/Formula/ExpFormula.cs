using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Formulaes/Exponential")]
public class ExpFormula : AbilityFormula
{
    public override float Evaluate(int level)
    {
        return baseValue * Mathf.Pow(multiplier, level - 1);
    }
}
