using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyUpgrade : Upgrade
{
    public float capacityIncrease { get; private set; }

    public EnergyUpgrade(
        int cost, int level,
        float capacityIncrease) : base(cost, level)
    {
        this.capacityIncrease = capacityIncrease;
    }

}
