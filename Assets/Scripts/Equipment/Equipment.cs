using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{

    public string name { get; private set; }
    public int cost { get; private set; }
    public float level { get; set; }
    public List<Upgrade> upgrades { get; private set; }

    [HideInInspector]
    public bool isEnabled = false;

    public virtual void Initialize(string name, int cost, int level)
    {
        this.name = name;
        this.cost = cost;
        this.level = level;
        upgrades = new List<Upgrade>();
    }

    //function prototype for EnergyEquipment class
    public virtual void Initialize(string name, int cost, int level, float energyCapacity, float energy, float depletionRate) { }
    //function prototype for the TransmitterEquipment class
    public virtual void Initialize(string name, int cost, int level, float uploadFactor) { }
    //function prototype for the WeaponEquipment class
    public virtual void Initialize(string name, int cost, int level, float damage, float inaccuracy, float fireRate, float range){ }
    //function prototype for the StorageEquipment class
    public virtual void Initialize(string name, int cost, int level, int capacity) { }
    //function prototype for the ResearchEquipment class
    public virtual void Initialize(string name, int cost, int level, ResearchTypes type, float scanTime, float range) { }

    public virtual void AddUpgrade(Upgrade upgrade)
    {
        upgrades.Add(upgrade);
    }
}
