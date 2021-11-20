using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyEquipment : Equipment
{
    public float energyCapacity { get; set; }
    public float energy { get; set; }

    public float depletionRate { get; private set; }

    public Slider energyBarSlider;

    public override void Initialize(string name, int cost, int level, float energyCapacity, float energy, float depletionRate)
    {
        base.Initialize(name, cost, level);
        this.energyCapacity = energyCapacity;
        this.energy = energy;
        this.depletionRate = depletionRate;
    }

    public void SetEnergyBarSlider(Slider _energyBarSlider)
    {
        energyBarSlider = _energyBarSlider;
        energyBarSlider.maxValue = energyCapacity;
    }


    public override void AddUpgrade(Upgrade upgrade)
    {
        if (upgrade is EnergyUpgrade)
        {
            upgrades.Add(upgrade);
            EnergyUpgrade energyUpgrade = (EnergyUpgrade)upgrade;

            level = energyUpgrade.level;
            energyCapacity = energyUpgrade.capacityIncrease;
            energyBarSlider.maxValue = energyCapacity;
        }
    }

    private void Update()
    {
        if (isEnabled)
        {
            energy -= depletionRate * Time.deltaTime;
            if (energyBarSlider != null)
            {
                energyBarSlider.value = energy;
            }
            if (energy <= 0)
            {
                if (!PlayerManager.instance.isDead)
                {
                    PlayerManager.instance.RanOutOfEnergy();
                }
            }
        }


    }



}
