using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{

    private float damageModifier = 1.1f;

    private float inaccuracyModifier = 0.909f;

    private float firerateModifier = 0.909f;

    private float rangeModifier = 1.1f;

    private float energyCapacityModifier = 1.1f;

    private float uploadFactorModifier = 0.909f;

    private float scanTimeModifier = 0.909f;

    private float scanRangeModifier = 1.1f;
    private int costModifier = 50;

    private List<Equipment> equipment;
    public List<Upgrade> upgrades = new List<Upgrade>();
    private List<ValueType> upgradeStructs = new List<ValueType>();
    private List<int> structVals = new List<int>();

    public static UpgradeManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    struct WeaponUpgradeStruct
    {
        public int cost;
        public int levelUpgrade;
        public float damageUpgrade;
        public float inaccuracryUpgrade;
        public float fireRateUpgrade;
        public float rangeUpgrade;

        public WeaponUpgradeStruct(
            int _cost, int _levelUpgrade, float _damageUpgrade, 
            float _inaccuracyUpgrade, float _fireRateUpgrades, float _rangeUpgrade)
        {
            cost = _cost;
            levelUpgrade = _levelUpgrade;
            damageUpgrade = _inaccuracyUpgrade;
            inaccuracryUpgrade = _inaccuracyUpgrade;
            fireRateUpgrade = _fireRateUpgrades;
            rangeUpgrade = _rangeUpgrade;
        }

        public void Upgrade()
        {
            cost += instance.costModifier;
            levelUpgrade++;
            damageUpgrade *= instance.damageModifier;
            inaccuracryUpgrade *= instance.inaccuracyModifier;
            fireRateUpgrade *= instance.firerateModifier;
            rangeUpgrade *= instance.rangeModifier;
        }
    }

    struct StorageUpgradeStruct
    {
        public int cost;
        public int levelUpgrade;
        public int capacityUpgrade;

        public StorageUpgradeStruct(int _cost, int _levelUpgrade, int _capacityUpgrade)
        {
            cost = _cost;
            levelUpgrade = _levelUpgrade;
            capacityUpgrade = _capacityUpgrade;
        }

        public void Upgrade()
        {
            cost += instance.costModifier;
            levelUpgrade++;
            capacityUpgrade++;
        }
    }

    struct EnergyUpgradeStruct
    {
        public int cost;
        public int levelUpgrade;
        public float energyCapacityUpgrade;

        public EnergyUpgradeStruct(int _cost, int _levelUpgrade, float _energyCapacityUpgrade)
        {
            cost = _cost;
            levelUpgrade = _levelUpgrade;
            energyCapacityUpgrade = _energyCapacityUpgrade;
        }

        public void Upgrade()
        {
            cost += instance.costModifier;
            levelUpgrade++;
            energyCapacityUpgrade *= instance.energyCapacityModifier;
        }
    }

    struct TransUpgradeStruct
    {
        public int cost;
        public int levelUpgrade;
        public float uploadFactorUpgrade;

        public TransUpgradeStruct(int _cost, int _levelUpgrade, float _uploadFactorUpgrade)
        {
            cost = _cost;
            levelUpgrade = _levelUpgrade;
            uploadFactorUpgrade = _uploadFactorUpgrade;
        }

        public void Upgrade()
        {
            cost += instance.costModifier;
            levelUpgrade++;
            uploadFactorUpgrade *= instance.uploadFactorModifier;
        }
    }

    public struct ResearchUpgradeStruct
    {
        public int cost;
        public int levelUpgrade;
        public float scanTimeUpgrade;
        public float scanRangeUpgrade;

        public ResearchUpgradeStruct(int _cost, int _levelUpgrade, float _scanTimeUpgrade, float _scanRangeUpgrade)
        {
            cost = _cost;
            levelUpgrade = _levelUpgrade;
            scanTimeUpgrade = _scanTimeUpgrade;
            scanRangeUpgrade = _scanRangeUpgrade;
        }

        public void Upgrade()
        {
            cost += instance.costModifier;
            levelUpgrade++;
            scanTimeUpgrade *= instance.scanTimeModifier;
            scanRangeUpgrade *= instance.scanRangeModifier;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeStats();
        CreateIntialUpgrades();
    
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InitializeStats()
    {
        foreach (Equipment eq in PlayerManager.instance.equipment)
        {

            if (eq is WeaponEquipment)
            {
                WeaponEquipment w = eq as WeaponEquipment;
                WeaponUpgradeStruct wu = new WeaponUpgradeStruct(800, 1, w.damage, w.inaccuracy, w.fireRate, w.range);
                wu.Upgrade();
                upgradeStructs.Add(wu);
                structVals.Add(0);
            }
            else if (eq is StorageEquipment)
            {
                StorageEquipment s = eq as StorageEquipment;
                StorageUpgradeStruct su = new StorageUpgradeStruct(700, 1, s.capacity);
                su.Upgrade();
                upgradeStructs.Add(su);
                structVals.Add(1);
            }
            else if (eq is EnergyEquipment)
            {
                EnergyEquipment e = eq as EnergyEquipment;
                EnergyUpgradeStruct ee = new EnergyUpgradeStruct(850, 1, e.energyCapacity);
                ee.Upgrade();
                upgradeStructs.Add(ee);
                structVals.Add(2);
            }
            else if (eq is TransmitterEquipment)
            {
                TransmitterEquipment t = eq as TransmitterEquipment;
                TransUpgradeStruct tu = new TransUpgradeStruct(700, 1, t.uploadFactor);
                tu.Upgrade();
                upgradeStructs.Add(tu);
                structVals.Add(3);
            }
            else
            {
                ResearchEquipment r = eq as ResearchEquipment;
                ResearchUpgradeStruct ru = new ResearchUpgradeStruct(750, 1, r.scanTime, r.range);
                ru.Upgrade();
                upgradeStructs.Add(ru);
                structVals.Add(4);
            }
        }
    }

    private void CreateIntialUpgrades()
    {
        foreach(ValueType vu in upgradeStructs) 
        {
            if (vu is WeaponUpgradeStruct)
            {
                WeaponUpgradeStruct ws = (WeaponUpgradeStruct)vu;
                upgrades.Add(new WeaponUpgrade(
                    ws.cost, ws.levelUpgrade, ws.damageUpgrade, ws.inaccuracryUpgrade, ws.fireRateUpgrade, ws.rangeUpgrade));
            }
            else if (vu is StorageUpgradeStruct)
            {
                StorageUpgradeStruct ss = (StorageUpgradeStruct)vu;
                upgrades.Add(new StorageUpgrade(
                    ss.cost, ss.levelUpgrade, ss.capacityUpgrade));
            }
            else if (vu is EnergyUpgradeStruct)
            {
                EnergyUpgradeStruct es = (EnergyUpgradeStruct)vu;
                upgrades.Add(new EnergyUpgrade(
                    es.cost, es.levelUpgrade, es.energyCapacityUpgrade));
            }
            else if (vu is TransUpgradeStruct)
            {
                TransUpgradeStruct ts = (TransUpgradeStruct)vu;
                upgrades.Add(new TransmitterUpgrade(
                    ts.cost, ts.levelUpgrade, ts.uploadFactorUpgrade));
            }
            else 
            {
                ResearchUpgradeStruct rs = (ResearchUpgradeStruct)vu;
                upgrades.Add(new ResearchUpgrade(
                    rs.cost, rs.levelUpgrade, rs.scanTimeUpgrade, rs.scanRangeUpgrade));
            }
        }
    }

    public void UpdateUpgrades(int index)
    {
        int structType = structVals[index];
        
        if (structType == 0)
        {
            WeaponUpgradeStruct wu = (WeaponUpgradeStruct)upgradeStructs[index];
            wu.Upgrade();
            upgradeStructs[index] = wu;
            upgrades[index] = new WeaponUpgrade(
                wu.cost, wu.levelUpgrade, wu.damageUpgrade, wu.inaccuracryUpgrade, wu.fireRateUpgrade, wu.rangeUpgrade);

        }
        else if (structType == 1)
        {
            StorageUpgradeStruct su = (StorageUpgradeStruct)upgradeStructs[index];
            su.Upgrade();
            upgradeStructs[index] = su;
            upgrades[index] = new StorageUpgrade(
                su.cost, su.levelUpgrade, su.capacityUpgrade);
        }
        else if (structType == 2)
        {
            EnergyUpgradeStruct eu = (EnergyUpgradeStruct)upgradeStructs[index];
            eu.Upgrade();
            upgradeStructs[index] = eu;
            upgrades[index] = new EnergyUpgrade(
                eu.cost, eu.levelUpgrade, eu.energyCapacityUpgrade);
        }
        else if (structType == 3)
        {
            TransUpgradeStruct tu = (TransUpgradeStruct)upgradeStructs[index];
            tu.Upgrade();
            upgradeStructs[index] = tu;
            upgrades[index] = new TransmitterUpgrade(
                tu.cost, tu.levelUpgrade, tu.uploadFactorUpgrade);
        }
        else
        {
            ResearchUpgradeStruct ru = (ResearchUpgradeStruct)upgradeStructs[index];
            ru.Upgrade();
            upgradeStructs[index] = ru;
            upgrades[index] = new ResearchUpgrade(
                ru.cost, ru.levelUpgrade, ru.scanTimeUpgrade, ru.scanRangeUpgrade);
        }
    }
}
