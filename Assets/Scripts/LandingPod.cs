using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandingPod : MonoBehaviour
{

    public float refuelRange = 5f;
    public float energyCapacity = 100f;
    public float energy { get; private set; }

    public float refuelRate = 1f;

    public Slider energyBar;

    public Transform roverTransform;
    public EnergyEquipment roverEnergyEquipment;

    private bool isActive = false;

    private int spawnIndex = -1;


    public Animator animator;
    public GameObject particleEmission;
    // Start is called before the first frame update
    void Start()
    {
        ////wait to initialize to make sure that the player manager object has been created
        //Invoke("Initialize", 1f);
        GetComponent<AudioSource>().volume *= AppManager.instance.fxVolume;
    }

    public void Initialize()
    {
        
        energy = energyCapacity;
        energyBar.maxValue = energyCapacity;
        energyBar.value = energy;
        roverTransform = GameObject.FindObjectOfType<RoverController>().transform;
        roverEnergyEquipment = PlayerManager.instance.GetComponent<EnergyEquipment>();
        isActive = true;
    }

    public void SetSpawnInfo(int _index)
    {
        spawnIndex = _index;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (roverEnergyEquipment == null)
            {
                roverEnergyEquipment = PlayerManager.instance.GetComponent<EnergyEquipment>();
            }
            if (energy > 0f)
            {
                if (roverTransform == null || roverEnergyEquipment == null) { return; }
                
                if ((roverTransform.position - transform.position).sqrMagnitude <= refuelRange * refuelRange)
                {
                    if (energy - refuelRate * Time.deltaTime < 0)
                    {
                        if (roverEnergyEquipment.energy + energy < roverEnergyEquipment.energyCapacity)
                        {
                            roverEnergyEquipment.energy += energy;
                            energy = 0f;
                        }
                    }
                    else
                    {
                        if (roverEnergyEquipment.energy + refuelRate * Time.deltaTime < roverEnergyEquipment.energyCapacity)
                        {
                            roverEnergyEquipment.energy += refuelRate * Time.deltaTime;
                            energy -= refuelRate * Time.deltaTime;
                        }
                    }
                    energyBar.value = energy;
                    
                }
            }
            else
            {
                isActive = false;
                if (spawnIndex != -1)
                {
                    if (GameManager.instance != null)
                    {
                        GameManager.instance.GetComponent<LandingPodSpawner>().PodDestroyed(spawnIndex);
                    }
                }
                Destroy(this.gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, refuelRange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!animator.GetBool("Landed"))
        {
            particleEmission.SetActive(true);
            animator.SetBool("Landed", true);
        }
    }
}
