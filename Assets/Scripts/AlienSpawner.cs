using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienSpawner : MonoBehaviour
{

    public GameObject alienPrefab;

    private Transform planetTransform;
    private float planetRadius;
    private Transform roverTransform;
    private DayNightCyle dayNightCycle;

    public List<Alien> aliveAliens = new List<Alien>();
    public LayerMask alienLayerMask;

    public int startingAlienCount = 5;
    public int maxAlienCount = 60;
    //the minimum distance that an alien can spawn from another alien
    public float alienNearbySpawnRange = 3f;
    public float alienRoverSpawnRange = 10f;
    //how many aliens to add each day. Right now this a constant number.
    public int dayAlienIncrementation = 1;

    private int currentDayAlienMaxCount = 5;

    private int currentDay = 0;

    private GameObject alienSpawnParent;

    bool isEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        planetTransform = FindObjectOfType<PlanetGravity>().transform;
        planetRadius = planetTransform.GetComponent<SphereCollider>().radius * planetTransform.localScale.x;
        roverTransform = FindObjectOfType<RoverController>().transform;
        dayNightCycle = FindObjectOfType<DayNightCyle>();
        currentDayAlienMaxCount = startingAlienCount;
        alienSpawnParent = new GameObject("Aliens");
        //spawn initial aliens
        for (int i = 0; i < startingAlienCount; i++)
        {
            SpawnAlien();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
        {
            //if a day has passed, spawn more aliens
            if (dayNightCycle.currentDay != currentDay)
            {
                currentDayAlienMaxCount += dayAlienIncrementation;
                currentDayAlienMaxCount = Mathf.Clamp(currentDayAlienMaxCount, 0, maxAlienCount);
                currentDay = dayNightCycle.currentDay;
                int numberOfAliens = aliveAliens.Count;

                for (int i = 0; i < currentDayAlienMaxCount - numberOfAliens; i++)
                {
                    SpawnAlien();
                }
            }
        }
    }


    void SpawnAlien()
    {
        //keep track of how many times we've tried to find a spawn point
        int attemptCounter = 0;
        Vector3 spawnLocation = Random.onUnitSphere * (planetRadius + 5);
        Collider[] nearbyAliens = Physics.OverlapSphere(spawnLocation, alienNearbySpawnRange, alienLayerMask);
        while (nearbyAliens.Length > 0 || Vector3.SqrMagnitude(spawnLocation - roverTransform.position) <= alienRoverSpawnRange * alienRoverSpawnRange)
        {
            attemptCounter++;
            if (attemptCounter == 15)
            {
                //it's not worth looking anymore. The planet must have too many aliens currently
                return;
            }
            spawnLocation = Random.onUnitSphere * (planetRadius + 5);
            nearbyAliens = Physics.OverlapSphere(spawnLocation, alienNearbySpawnRange, alienLayerMask);
        }
        
        //spawn the alien
        Alien alien = Instantiate(alienPrefab, spawnLocation, Quaternion.identity,alienSpawnParent.transform).GetComponent<Alien>();
        alien.SetSpawnInfo(aliveAliens.Count);
        aliveAliens.Add(alien);
    }

    //Remove alien from array when the alien dies
    public void AlienDied(int _index)
    {
        if (isEnabled)
        {
            aliveAliens.RemoveAt(_index);
        }
    }

    //called when game ends
    public void Disable()
    {
        isEnabled = false;
        foreach (Alien alien in aliveAliens)
        {
            //only destroy the alien component so that the alien mesh is still there
            Destroy(alien);
        }
    }
}
