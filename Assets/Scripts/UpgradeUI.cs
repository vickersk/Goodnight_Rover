using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public Text fundingText;
    public Text supportPodText;
    public GameObject upgradeInventory;
    public GameObject upgradeWindow;
    private GameObject upgradePrefab;
    private GameObject slot;
    public Sprite weaponImage;
    public Sprite transmitterImage;
    public Sprite batteryImage;
    public Sprite storageImage;
    public Sprite geologyImage;
    public Sprite archeologyImage;
    public Sprite biologyImage;
    private bool toggle = false;
    private List<Equipment> equipment;
    private List<Upgrade> possibleUpgrades = new List<Upgrade>();
    private List<GameObject> slots = new List<GameObject>();
    private UpgradeManager upgradeManager = UpgradeManager.instance;
    private AppManager app;


    public static UpgradeUI instance;
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

    void Start()
    {
        app = FindObjectOfType<AppManager>();

        upgradeInventory.SetActive(toggle);
        fundingText.text = String.Format("Current Funding:\n${0}", app.funding);

        upgradePrefab = (GameObject)Resources.Load("UpgradeSlot");

        equipment = PlayerManager.instance.equipment;

        for(int i = 0; i < equipment.Count; i++)
        {
            slot = Instantiate(upgradePrefab) as GameObject;
            slots.Add(slot);

            slot.transform.SetParent(upgradeWindow.transform, false);

            Text nameText = slot.transform.GetChild(0).GetComponent<Text>();
            nameText.text = String.Format("{0}", equipment[i].name);
            Text levelText = slot.transform.GetChild(1).GetComponent<Text>();
            Text statText = slot.transform.GetChild(2).GetComponent<Text>();
            Text costText = slot.transform.GetChild(3).transform.GetChild(0).GetComponent<Text>();
            Image icon = slot.transform.GetChild(4).GetComponent<Image>();
            

            if (equipment[i] is WeaponEquipment)
            {
                
                WeaponUpgrade u = (WeaponUpgrade)UpgradeManager.instance.upgrades[i];
                levelText.text = String.Format("Level: {0}", u.level);
                statText.text = String.Format(
                    "Damage: {0}\n" +
                    "Accuracy: {1}\n" +
                    "Firerate: {2}\n" +
                    "Range: {3}\n", 
                    u.damageIncrease, u.accuracyIncrease, u.fireRateIncrease, u.rangeIncrease);
                costText.text = String.Format("${0}", u.cost);
                icon.sprite = weaponImage;
                    
            }
            else if (equipment[i] is StorageEquipment)
            {
                StorageUpgrade u = (StorageUpgrade)UpgradeManager.instance.upgrades[i];
                levelText.text = String.Format("Level: {0}", u.level);
                statText.text = String.Format("Storage Capacity: {0}\n", u.capacityIncrease);
                costText.text = String.Format("${0}", u.cost);
                icon.sprite = storageImage;
            }
            else if (equipment[i] is EnergyEquipment)
            {
                EnergyUpgrade u = (EnergyUpgrade)UpgradeManager.instance.upgrades[i];
                levelText.text = String.Format("Level: {0}", u.level);
                statText.text = String.Format("Energy Capacity: {0}\n", u.capacityIncrease);
                costText.text = String.Format("${0}", u.cost);
                icon.sprite = batteryImage;
            }
            else if (equipment[i] is TransmitterEquipment)
            {
                TransmitterUpgrade u = (TransmitterUpgrade)UpgradeManager.instance.upgrades[i];
                levelText.text = String.Format("Level: {0}", u.level);
                statText.text = String.Format("Upload Time: {0}\n", u.uploadMultiplier);
                costText.text = String.Format("${0}", u.cost);
                icon.sprite = transmitterImage;
            }
            else
            {
                ResearchUpgrade u = (ResearchUpgrade)UpgradeManager.instance.upgrades[i];
                levelText.text = String.Format("Level: {0}", u.level);
                statText.text = String.Format(
                    "Scan Time: {0}\nRange: {1}\n", u.timeReduction, u.rangeIncrease);
                costText.text = String.Format("${0}", u.cost);
                
                if (equipment[i].name.IndexOf("Geological") != -1)
                {
                    
                    icon.sprite = geologyImage;
                }
                else if (equipment[i].name.IndexOf("Biological") != -1)
                {
                    
                    icon.sprite = biologyImage;
                }
                else if (equipment[i].name.IndexOf("Archeological") != -1)
                {
                    icon.sprite = archeologyImage;
                }
            }
        }
    }

    void Update()
    {
        upgradeInventory.SetActive(toggle);
        fundingText.text = String.Format("Current Funding:\n${0}", app.funding);
    }

    public void SetUpgradeMenu(bool toggle)
    {
        this.toggle = toggle;
    }

    public void UpdateUI()
    {
        for(int i = 0; i < slots.Count; i++)
        {
        
            Text nameText = slots[i].transform.GetChild(0).GetComponent<Text>();
            nameText.text = String.Format("{0}", equipment[i].name);
            Text levelText = slots[i].transform.GetChild(1).GetComponent<Text>();
            Text statText = slots[i].transform.GetChild(2).GetComponent<Text>();
            Text costText = slots[i].transform.GetChild(3).transform.GetChild(0).GetComponent<Text>();
            

            if (equipment[i] is WeaponEquipment)
            {
                
                WeaponUpgrade u = (WeaponUpgrade)UpgradeManager.instance.upgrades[i];
                levelText.text = String.Format("Level: {0}", u.level);
                statText.text = String.Format(
                    "Damage: {0}\n" +
                    "Accuracy: {1}\n" +
                    "Firerate: {2}\n" +
                    "Range: {3}\n", 
                    u.damageIncrease, u.accuracyIncrease, u.fireRateIncrease, u.rangeIncrease);
                costText.text = String.Format("${0}", u.cost);
                    
            }
            else if (equipment[i] is StorageEquipment)
            {
                StorageUpgrade u = (StorageUpgrade)UpgradeManager.instance.upgrades[i];
                levelText.text = String.Format("Level: {0}", u.level);
                statText.text = String.Format("Storage Capacity: {0}\n", u.capacityIncrease);
                costText.text = String.Format("${0}", u.cost);
            }
            else if (equipment[i] is EnergyEquipment)
            {
                EnergyUpgrade u = (EnergyUpgrade)UpgradeManager.instance.upgrades[i];
                levelText.text = String.Format("Level: {0}", u.level);
                statText.text = String.Format("Energy Capacity: {0}\n", u.capacityIncrease);
                costText.text = String.Format("${0}", u.cost);
            }
            else if (equipment[i] is TransmitterEquipment)
            {
                TransmitterUpgrade u = (TransmitterUpgrade)UpgradeManager.instance.upgrades[i];
                levelText.text = String.Format("Level: {0}", u.level);
                statText.text = String.Format("Upload Time: {0}\n", u.uploadMultiplier);
                costText.text = String.Format("${0}", u.cost);
            }
            else
            {
                ResearchUpgrade u = (ResearchUpgrade)UpgradeManager.instance.upgrades[i];
                levelText.text = String.Format("Level: {0}", u.level);
                statText.text = String.Format(
                    "Scan Time: {0}\nRange: {1}\n", u.timeReduction, u.rangeIncrease);
                costText.text = String.Format("${0}", u.cost);
            }
        }
    }
}
