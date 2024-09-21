using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Formulaes/Linear")]
public class LinearFormula : AbilityFormula
{
    public override float Evaluate(int level)
    {
        return baseValue + (multiplier * (level - 1));
    }
}
