using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpFormula : AbilityFormula
{
    public override float Evaluate(int level)
    {
        return baseValue * (multiplier ^ (level - 1));
    }
}
