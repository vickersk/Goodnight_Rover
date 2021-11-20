using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgrade : Upgrade
{
    public float damageIncrease { get; private set; }
    public float accuracyIncrease { get; private set; }
    public float fireRateIncrease { get; private set; }
    public float rangeIncrease { get; private set; }

    public WeaponUpgrade(
        int cost, int level,
        float damageIncrease, float accuracyIncrease, float fireRateIncrease,
        float rangeIncrease) : base (cost, level)
    {
        this.damageIncrease = damageIncrease;
        this.accuracyIncrease = accuracyIncrease;
        this.fireRateIncrease = fireRateIncrease;
        this.rangeIncrease = rangeIncrease;
    }

}
