using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchUpgrade : Upgrade
{
    public float timeReduction { get; private set; }
    public float rangeIncrease { get; private set; }

    public ResearchUpgrade(
        int cost, int level,
        float timeReduction, float rangeIncrease) : base(cost, level)
    {
        this.timeReduction = timeReduction;
        this.rangeIncrease = rangeIncrease;
    }

}
