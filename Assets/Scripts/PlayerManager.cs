using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


//struct to store information about research material
[Serializable]
public struct ResearchMaterialStruct
{
    public ResearchTypes researchType;
    public int worth;

    public ResearchMaterialStruct(ResearchTypes _type, int _worth)
    {
        researchType = _type;
        worth = _worth;
    }
}

public class PlayerManager : MonoBehaviour
{
    [Header("Constants")]
    [SerializeField]
    private float INITIAL_ENERGY = 100.0f;
    [SerializeField]
    private int INITIAL_CAPACITY = 6;
    [SerializeField]
    private float INITIAL_UPLOAD_FACTOR = 2.0f;
    [SerializeField]
    private float INITIAL_HEALTH = 100.0f;
    [SerializeField]
    private float ENERGY_DEPLETION_RATE = 2f;

    [Header("Material Energy Boosts")]
    public float biologyMaterialEnergyBoost = 5f;
    public float geologyMaterialEnergyBoost = 3f;
    public float archeologyMaterialEnergyBoost = 2f;


    public bool isDead = false;

    public static PlayerManager instance;

    //Singleton implementation
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



    public float energy { get; private set; }
    public float health { get; private set; }


    public List<Equipment> equipment = new List<Equipment>();

    public Slider energyBarSlider;
    public Slider healthBarSlider;

    public RectTransform nearByEnergyArrow;
    private float lastEnergyArrowUpdateTime = 0f;
    private float energyArrowUpdateDelay = 0.5f;
    private LandingPodSpawner landingPodSpawner;

    public List<ResearchMaterialStruct> pickedUpResearchMaterials = new List<ResearchMaterialStruct>();

    public RoverController rover;

    [Header("EndGame UI")]
    public GameObject mainCanvas;
    public GameObject endGameObject;
    public Text causeOfDeathText;
    public Text missionReportText;
    public Text highScoreText;

    public int closestLandingPodIndex = -1;
    public float nearbyEnergyArrowAngle = -1000f;

    private CustomizationManager customization;




    void Start()
    {
        
        gameObject.AddComponent<EnergyEquipment>();
        GetComponent<EnergyEquipment>().Initialize("Battery", 0, 1, INITIAL_ENERGY, INITIAL_ENERGY,ENERGY_DEPLETION_RATE);
        GetComponent<EnergyEquipment>().SetEnergyBarSlider(energyBarSlider);
        GetComponent<EnergyEquipment>().isEnabled = true;
        equipment.Add(GetComponent<EnergyEquipment>());

        gameObject.AddComponent<TransmitterEquipment>();
        GetComponent<TransmitterEquipment>().Initialize("Transmitter", 0, 1, INITIAL_UPLOAD_FACTOR);
        equipment.Add(GetComponent<TransmitterEquipment>());

        gameObject.AddComponent<StorageEquipment>();
        GetComponent<StorageEquipment>().Initialize("Sample Storage", 0, 1, INITIAL_CAPACITY);
        equipment.Add(GetComponent<StorageEquipment>());

        /*
        //for testing purposes include research equipment by default
        gameObject.AddComponent<ResearchEquipment>();
        GetComponent<ResearchEquipment>().Initialize("Geology Research Equipment", 0, 1, ResearchTypes.Geology, 2, 6f);
        equipment.Add(GetComponent<ResearchEquipment>());

        // To modify the weapon just adjust the values
        gameObject.AddComponent<WeaponEquipment>();
        GetComponent<WeaponEquipment>().Initialize("Pew Pew Machine", 0, 1, 0, 0.2f, 0.5f, 10f);
        equipment.Add(GetComponent<WeaponEquipment>());
        */

        customization = FindObjectOfType<CustomizationManager>();
        InitializeEquipment();

        health = INITIAL_HEALTH;

        rover = FindObjectOfType<RoverController>();
        landingPodSpawner = GetComponent<LandingPodSpawner>();


    }


    public bool CollectResearchMaterial(ResearchMaterial _researchMaterial)
    {
        if (_researchMaterial == null) { return false; }
        StorageEquipment storageEquipment = (StorageEquipment)equipment[2];
        if (pickedUpResearchMaterials.Count >= storageEquipment.capacity)
        {
            return false;
        }
        pickedUpResearchMaterials.Add(new ResearchMaterialStruct(_researchMaterial.GetResearchType(), _researchMaterial.GetWorth()));
        EnergyEquipment e = (EnergyEquipment)equipment[GetEnergyEquipment()];
        if (_researchMaterial.GetResearchType() == ResearchTypes.Biology)
        {
            e.energy += biologyMaterialEnergyBoost;
        }
        else if (_researchMaterial.GetResearchType() == ResearchTypes.Archeology)
        {
            e.energy += archeologyMaterialEnergyBoost;
        }
        else
        {
            e.energy += geologyMaterialEnergyBoost;
        }
        
        e.energy = Mathf.Clamp(e.energy, 0f, e.energyCapacity);
        Destroy(_researchMaterial.gameObject);
        return true;
    }

    public void TakeDamage(float damage)
    {
        if (!isDead)
        {
            health -= damage;
            healthBarSlider.value = health;
            if (health <= 0)
            {
                Debug.Log("Rover destroyed. Game over.");
                isDead = true;
                RoverDied();
            }
        }
    }

    public void RanOutOfEnergy()
    {
        if (!isDead)
        {
            Debug.Log("Rover ran out of energy. Game over.");
            isDead = true;
            RoverDied();
        }
    }


    public void RoverDied()
    {
        isDead = true;
        rover.Disable();
        GetComponent<DayNightCyle>().Disable();
        GetComponent<AlienSpawner>().Disable();
        GetComponent<ResearchMaterialSpawner>().Disable();
        GetComponent<LandingPodSpawner>().Disable();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        //hide all UI
        foreach (Transform child in mainCanvas.transform)
        {
            child.gameObject.SetActive(false);
        }

        //Setup endgame UI
        if (health <= 0)
        {
            causeOfDeathText.text = "The rover was destroyed.";
        }
        else
        {
            causeOfDeathText.text = "The rover ran out of energy.";
        }
        missionReportText.text = String.Format("The mission lasted {0} days.", GetComponent<DayNightCyle>().currentDay);
        //set score
        AppManager appManager = FindObjectOfType<AppManager>();
        if (appManager != null)
        {
            appManager.SetHighScore(GetComponent<DayNightCyle>().currentDay);
        }

        highScoreText.text = String.Format("High Score: {0}", appManager.GetHighScore());
        //show endgame UI
        endGameObject.SetActive(true);
    }

    private void InitializeEquipment()
    {
        int[] numEquipment = customization.numEquipment;

        for (int i = 0; i < numEquipment[0]; i++)
        {
            WeaponEquipment weaponEquipment = gameObject.AddComponent<WeaponEquipment>();
            weaponEquipment.Initialize("Turret", 0, 1, 0, 0.2f, 0.5f, 10f);
            equipment.Add(weaponEquipment);
        }

        for (int i = 0; i < numEquipment[1]; i++)
        {
            ResearchEquipment researchEquipment =  gameObject.AddComponent<ResearchEquipment>();
            researchEquipment.Initialize("Geological Research Equipment", 0, 1, ResearchTypes.Geology, 2, 6f);
            equipment.Add(researchEquipment);
        }

        for (int i = 0; i < numEquipment[2]; i++)
        {
            ResearchEquipment researchEquipment = gameObject.AddComponent<ResearchEquipment>();
            researchEquipment.Initialize("Biological Research Equipment", 0, 1, ResearchTypes.Biology, 2, 6f);
            equipment.Add(researchEquipment);
        }

        for (int i = 0; i < numEquipment[3]; i++)
        {
            ResearchEquipment researchEquipment = gameObject.AddComponent<ResearchEquipment>();
            researchEquipment.Initialize("Archeological Research Equipment", 0, 1, ResearchTypes.Archeology, 2, 6f);
            equipment.Add(researchEquipment);
        }
    }

    public int GetWeaponEquipment()
    {
        
        
        for (int i = 0; i < equipment.Count; i++)
        {
            if (equipment[i] is WeaponEquipment)
            {
                return i;
            }
        }

        return -1;
    }

    public int GetEnergyEquipment()
    {
        for (int i = 0; i < equipment.Count; i++)
        {
            if (equipment[i] is EnergyEquipment)
            {
                return i;
            }
        }

        return -1;
    }

    public int GetNumBio()
    {
        int counter = 0;

        foreach (ResearchMaterialStruct rm in pickedUpResearchMaterials)
        {
            if (rm.researchType is ResearchTypes.Biology)
            {
                counter++;
            }
        }

        return counter;
    }

    public int GetNumGeo()
    {
        int counter = 0;

        foreach (ResearchMaterialStruct rm in pickedUpResearchMaterials)
        {
            if (rm.researchType is ResearchTypes.Geology)
            {
                counter++;
            }
        }

        return counter;
    }

    public int GetNumArch()
    {
        int counter = 0;

        foreach (ResearchMaterialStruct rm in pickedUpResearchMaterials)
        {
            if (rm.researchType is ResearchTypes.Archeology)
            {
                counter++;
            }
        }

        return counter;
    }

    private void Update()
    {
        if (isDead) { return; }
        //all of this code points the blue arrow in the directino of the nearest landing pod
        if (Time.time - GameManager.instance.worldStartTime - lastEnergyArrowUpdateTime >= energyArrowUpdateDelay)
        {
            lastEnergyArrowUpdateTime = Time.time - GameManager.instance.worldStartTime;
            if (landingPodSpawner.currentPods.Count == 0)
            {
                if (nearByEnergyArrow.gameObject.activeSelf)
                {
                    nearByEnergyArrow.gameObject.SetActive(false);
                    closestLandingPodIndex = -1;
                    nearbyEnergyArrowAngle = -1000f;
                }
                return;
            }
            else
            {
                if (!nearByEnergyArrow.gameObject.activeSelf)
                {
                    nearByEnergyArrow.gameObject.SetActive(true);
                }
            }
            int closestPodIndex = -1;
            float closestPodDistance = 1000f;
            for (int i = 0; i < landingPodSpawner.currentPods.Count; i++)
            {
                if (landingPodSpawner.currentPods[i] == null) { continue; }
           
                if (Vector3.SqrMagnitude(landingPodSpawner.currentPods[i].transform.position - rover.transform.position) < closestPodDistance * closestPodDistance)
                {
                    closestPodDistance = Vector3.Distance(landingPodSpawner.currentPods[i].transform.position, rover.transform.position);
                    closestPodIndex = i;
                }
            }
            if (closestPodIndex == -1)
            {
                nearByEnergyArrow.gameObject.SetActive(false);
                return;
            }
            closestLandingPodIndex = closestPodIndex;
            LandingPod closestPod = landingPodSpawner.currentPods[closestPodIndex];
            if (closestPod == null)
            {
                Debug.LogError("closest landing pod was null");
            }
            Vector3 dirToPod = (closestPod.transform.position - rover.transform.position).normalized;
            Vector3 projectedDirection = Vector3.ProjectOnPlane(dirToPod, rover.transform.up);
            float ang = Vector3.SignedAngle(projectedDirection, rover.transform.forward, rover.transform.up);
            nearbyEnergyArrowAngle = ang;
            nearByEnergyArrow.eulerAngles = new Vector3(0, 0, ang);


        }
    }



}
