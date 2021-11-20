using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    public Text capacityText;
    public Text archSlotText;
    public Text bioSlotText;
    public Text geoSlotText;

    public Sprite weaponImage;
    public Sprite transmitterImage;
    public Sprite batteryImage;
    public Sprite storageImage;
    

    public Sprite archImage;
    public Sprite geoImage;
    public Sprite bioImage;

    public Image archImageSpot;
    public Image bioImageSpot;
    public Image geoImageSpot;


    // Temporary for testing
    public Text directions;

    private Text equipmentText;
    private Text levelText;
    private StorageEquipment storage;

    private GameObject equipmentParent;
    private GameObject equipmentPrefab;
    private GameObject slot;
    private List<GameObject> slots = new List<GameObject>();

    private int totalMaterials;
    private int totalCapacity;
    private int numArch;
    private int numBio;
    private int numGeo;
    private bool equipmentToggle = false;
    private ResearchTypes missingResearchType;

    public static InventoryUI instance;
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

    // Start is called before the first frame update
    void Start()
    {
    
        totalMaterials = PlayerManager.instance.pickedUpResearchMaterials.Count;
        storage = (StorageEquipment)PlayerManager.instance.equipment[2];
        totalCapacity = storage.capacity;
        capacityText.text = String.Format("Capacity: {0}/{1}", totalMaterials, totalCapacity);
        bool foundGeo = false;
        bool foundArch = false;
        bool foundBio = false;
        for (int i = 2; i <PlayerManager.instance.equipment.Count;i++)
        {
            if (PlayerManager.instance.equipment[i] is ResearchEquipment)
            {
                ResearchEquipment e = (ResearchEquipment)PlayerManager.instance.equipment[i];
                if (e.type == ResearchTypes.Geology)
                {
                    foundGeo = true;
                }
                else if (e.type == ResearchTypes.Archeology)
                {
                    foundArch = true;
                }
                else
                {
                    foundBio = true;
                }
            }
        }
        if (!foundGeo)
        {
            missingResearchType = ResearchTypes.Geology;
            archImageSpot.sprite = archImage;
            bioImageSpot.sprite = bioImage;
        }
        else if (!foundBio)
        {
            missingResearchType = ResearchTypes.Biology;
            archImageSpot.sprite = archImage;
            geoImageSpot.sprite = geoImage;
        }
        else if (!foundArch)
        {
            missingResearchType = ResearchTypes.Archeology;
            bioImageSpot.sprite = bioImage;
            geoImageSpot.sprite = geoImage;
        }
        
        geoSlotText.text = "";
        bioSlotText.text = "";
        archSlotText.text = "";


        equipmentPrefab = (GameObject)Resources.Load("EquipmentSlot");
        equipmentParent = this.gameObject.transform.GetChild(1).gameObject;

        foreach(Equipment e in PlayerManager.instance.equipment)
        {
            slot = Instantiate(equipmentPrefab) as GameObject;
            slot.transform.SetParent(equipmentParent.transform, false);
            slots.Add(slot);
            equipmentText = slot.transform.GetChild(0).GetComponent<Text>();
            levelText = slot.transform.GetChild(1).GetComponent<Text>();
            equipmentText.text = String.Format("{0}", e.name);
            levelText.text = String.Format("Level: {0}", e.level);
            Image icon = slot.transform.GetChild(2).GetComponent<Image>();
            if (e is TransmitterEquipment)
            {
                slot.transform.GetChild(2).GetComponent<Image>().sprite = transmitterImage;
            }
            else if (e is EnergyEquipment)
            {
                slot.transform.GetChild(2).GetComponent<Image>().sprite = batteryImage;
            }
            else if (e is StorageEquipment)
            {
                slot.transform.GetChild(2).GetComponent<Image>().sprite = storageImage;
            }
            else if (e is WeaponEquipment)
            {
                slot.transform.GetChild(2).GetComponent<Image>().sprite = weaponImage;
            }
            else if (e is ResearchEquipment)
            {
                ResearchEquipment rE = (ResearchEquipment)e;
                if (rE.type == ResearchTypes.Archeology)
                {
                    slot.transform.GetChild(2).GetComponent<Image>().sprite = archImage;
                }
                else if (rE.type == ResearchTypes.Biology)
                {
                    slot.transform.GetChild(2).GetComponent<Image>().sprite = bioImage;
                }
                else if (rE.type == ResearchTypes.Geology)
                {
                    slot.transform.GetChild(2).GetComponent<Image>().sprite = geoImage;
                }
            }
        
        }

        equipmentParent.SetActive(equipmentToggle);
    }

    // Update is called once per frame
    void Update()
    {
        totalMaterials = PlayerManager.instance.pickedUpResearchMaterials.Count;
        storage = (StorageEquipment)PlayerManager.instance.equipment[2];
        totalCapacity = storage.capacity;
        capacityText.text = String.Format("Capacity: {0}/{1}", totalMaterials, totalCapacity);

        if (missingResearchType != ResearchTypes.Geology)
        {
            geoSlotText.text = String.Format("{0}", PlayerManager.instance.GetNumGeo());
        }
        if (missingResearchType != ResearchTypes.Biology)
        {
            bioSlotText.text = String.Format("{0}", PlayerManager.instance.GetNumBio());
        }
        if (missingResearchType != ResearchTypes.Archeology)
        {
            archSlotText.text = String.Format("{0}", PlayerManager.instance.GetNumArch());
        }
        

        for (int i = 0; i < slots.Count; i++)
        {
            Equipment e = PlayerManager.instance.equipment[i];
            levelText = slots[i].transform.GetChild(1).GetComponent<Text>();
            levelText.text = String.Format("Level: {0}", e.level);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            equipmentToggle = !equipmentToggle;
            equipmentParent.SetActive(equipmentToggle);
            directions.enabled = !directions.enabled;
        }  
    }
}
