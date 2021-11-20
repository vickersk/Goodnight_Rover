using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationManager : MonoBehaviour
{

    public int totalSlots { get; private set; }
    public int openSlots { get; set; }

    private const int TOTAL_SLOTS = 3;

    private int numEnergy;
    private int numStorage;
    private int numTrans;
    private int numWeapon;
    private int numGeo;
    private int numBio;
    private int numArch;
    public List<Equipment> selectedEquipment = new List<Equipment>();

    public int[] numEquipment = { 0, 0, 0, 0 };
    public int[] maxEquipment = {3, 1, 1, 1 };

    private float INITIAL_ENERGY = 100.0f;
    private int INITIAL_CAPACITY = 6;
    private float INITIAL_UPLOAD_FACTOR = 2.0f;
    private float INITIAL_HEALTH = 100.0f;
    private float ENERGY_DEPLETION_RATE = 2f;

    public static CustomizationManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //make sure that this object is not destroyed when switching between scenes
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        openSlots = 3;
        totalSlots = TOTAL_SLOTS;

        gameObject.AddComponent<EnergyEquipment>();
        GetComponent<EnergyEquipment>().Initialize("Battery", 0, 1, INITIAL_ENERGY, INITIAL_ENERGY, ENERGY_DEPLETION_RATE);
        selectedEquipment.Add(GetComponent<EnergyEquipment>());

        gameObject.AddComponent<TransmitterEquipment>();
        GetComponent<TransmitterEquipment>().Initialize("Transmitter", 0, 1, INITIAL_UPLOAD_FACTOR);
        selectedEquipment.Add(GetComponent<TransmitterEquipment>());

        gameObject.AddComponent<StorageEquipment>();
        GetComponent<StorageEquipment>().Initialize("Sample Storage", 0, 1, INITIAL_CAPACITY);
        selectedEquipment.Add(GetComponent<StorageEquipment>());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddEquipment(int type)
    {
        if (openSlots != 0)
        {
            switch (type)
            {
                case 0:
                    if (numEquipment[0] < maxEquipment[0])
                    {
                        openSlots--;
                        numEquipment[0]++;
                    }
                    break;

                case 1:
                    if (numEquipment[1] < maxEquipment[1])
                    {
                        openSlots--;
                        numEquipment[1]++;
                    }
                    break;

                case 2:
                    if (numEquipment[2] < maxEquipment[2])
                    {
                        openSlots--;
                        numEquipment[2]++;
                    }
                    break;

                case 3:
                    if (numEquipment[3] < maxEquipment[3])
                    {
                        openSlots--;
                        numEquipment[3]++;

                    }
                    break;
            }
        }
    }

    public void RemoveEquipment(int type)
    {
        switch (type)
        {
            case 0:

                if (numEquipment[0] > 0)
                {
                    numEquipment[0]--;
                    openSlots++;
                }

                break;

            case 1:

                if (numEquipment[1] > 0)
                {
                    numEquipment[1]--;
                    openSlots++;
                }
                break;

            case 2:

                if (numEquipment[2] > 0)
                {
                    numEquipment[2]--;
                    openSlots++;
                }
                break;

            case 3:

                if (numEquipment[3] > 0)
                {
                    numEquipment[3]--;
                    openSlots++;
                }
                break;
        }
    }

    /*
    public void InitializeEquipment()
    {
        int researchCounter = 0;

        for (int i = 0; i < numEquipment[0]; i++)
        {
            go.AddComponent<WeaponEquipment>();
            GetComponent<WeaponEquipment>().Initialize("Turret", 0, 1, 0, 0.2f, 0.5f, 10f);

            if (go == this.gameObject){
                selectedEquipment.Add(GetComponent<WeaponEquipment>());
            }
        }

        for (int i = 0; i < numEquipment[1]; i++)
        {
            go.AddComponent<ResearchEquipment>();
            GetComponent<ResearchEquipment>().Initialize("Geological Research Equipment", 0, 1, ResearchTypes.Geology, 2, 6f);
            
            if (go == this.gameObject){
                selectedEquipment.Add(GetComponent<ResearchEquipment>());
            }
            researchCounter++;
        }

        for (int i = 0; i < numEquipment[2]; i++)
        {
            go.AddComponent<ResearchEquipment>();
            GetComponent<ResearchEquipment>().Initialize("Biological Research Equipment", 0, 1, ResearchTypes.Geology, 2, 6f);
            if (go == this.gameObject){
                selectedEquipment.Add(GetComponent<ResearchEquipment>());
            }
            
        }

        for (int i = 0; i < numEquipment[3]; i++)
        {
            go.AddComponent<ResearchEquipment>();
            GetComponent<ResearchEquipment>().Initialize("Archeological Research Equipment", 0, 1, ResearchTypes.Geology, 2, 6f);
            selectedEquipment.Add(GetComponent<ResearchEquipment>());
        }
    }
    */

    public bool IsAvailable(int index)
    {
        return (openSlots != 0 && numEquipment[index] < maxEquipment[index]); 
    }
}



