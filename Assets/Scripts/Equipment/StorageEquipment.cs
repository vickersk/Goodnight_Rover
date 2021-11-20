using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageEquipment : Equipment
{
    public int capacity { get; private set; }

    public override void Initialize(string name, int cost, int level, int capacity)
    {
        base.Initialize(name, cost, level);
        this.capacity = capacity;
    }


    public override void AddUpgrade(Upgrade upgrade)
    {
        if (upgrade is StorageUpgrade)
        {
            upgrades.Add(upgrade);
            StorageUpgrade storageUpgrade = (StorageUpgrade)upgrade;

            level = storageUpgrade.level;
            capacity = storageUpgrade.capacityIncrease;
        }
    }
}
