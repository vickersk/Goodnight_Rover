using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageUpgrade : Upgrade
{
    public int capacityIncrease { get; private set; }

    public StorageUpgrade(
        int cost, int level, int capacityIncrease) : base(cost, level)
    {
        this.capacityIncrease = capacityIncrease;
    }

    
}
