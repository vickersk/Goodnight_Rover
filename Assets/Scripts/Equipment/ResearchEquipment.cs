using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResearchTypes
{
    Archeology,
    Biology,
    Geology
}

public class ResearchEquipment : Equipment
{



    public ResearchTypes type { get; private set; }
    public float scanTime { get; private set; }
    public float range { get; private set; }


    public override void Initialize(string name, int cost, int level, ResearchTypes type, float scanTime, float range)
    {
        base.Initialize(name, cost, level);
        this.type = type;
        this.scanTime = scanTime;
        this.range = range;

    }


    public override void AddUpgrade(Upgrade upgrade)
    {
        if (upgrade is ResearchUpgrade)
        {
            upgrades.Add(upgrade);
            ResearchUpgrade researchUpgrade = (ResearchUpgrade)upgrade;
            
            level = researchUpgrade.level;
            scanTime = researchUpgrade.timeReduction;
            range = researchUpgrade.rangeIncrease;
        }
    }
}
