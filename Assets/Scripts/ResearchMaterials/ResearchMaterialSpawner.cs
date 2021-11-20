using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchMaterialSpawner : MonoBehaviour
{

    public struct SpawnNode
    {
        public float destroyedTime;
        public int arrayIndex;

        public SpawnNode(float _destroyedTime,int _arrayIndex)
        {
            destroyedTime = _destroyedTime;
            arrayIndex = _arrayIndex;
        }
    }


    public int numBioMaterials;
    public int numArchMaterials;
    public int numGeoMaterials;

    public int maxMaterials = 100;

    public GameObject bioResearchMaterialPrefab;
    public GameObject archResearchMaterialPrefab;
    public GameObject geoResearchMaterialPrefab;

    private Transform planetTransform;
    private float planetRadius;
    private Transform roverTransform;

    [SerializeField]
    private int bioMaterialsToSpawn;
    [SerializeField]
    private int archMaterialsToSpawn;
    [SerializeField]
    private int geoMaterialsToSpawn;


    public Vector2 minmaxRespawnTime;
    public float minRoverDistanceToRespawn = 10f;


    private List<Vector3> spawnPoints = new List<Vector3>();
    private List<bool> spawnPointAvailability = new List<bool>();

    public List<SpawnNode> spawnQueue = new List<SpawnNode>();

    private GameObject researchMaterialSpawnParent;

    private bool isEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        planetTransform = FindObjectOfType<PlanetGravity>().transform;
        planetRadius = planetTransform.GetComponent<SphereCollider>().radius * planetTransform.localScale.x;
        roverTransform = FindObjectOfType<RoverController>().transform;
        bioMaterialsToSpawn = numBioMaterials;
        archMaterialsToSpawn = numArchMaterials;
        geoMaterialsToSpawn = numGeoMaterials;
        researchMaterialSpawnParent = new GameObject("ResearchMaterials");
        GenerateSpawnPoints();
        InitialMaterialSpawning();

    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
        {
            //respawn research materials
            if (spawnQueue.Count > 0)
            {
                float respawnDelay = Random.Range(minmaxRespawnTime.x, minmaxRespawnTime.y);
                if (Time.time - GameManager.instance.worldStartTime - spawnQueue[0].destroyedTime >= respawnDelay && Vector3.SqrMagnitude(spawnPoints[spawnQueue[0].arrayIndex] - roverTransform.position) >= minRoverDistanceToRespawn * minRoverDistanceToRespawn)
                {
                    SpawnMaterial(spawnQueue[0].arrayIndex);
                    spawnQueue.RemoveAt(0);
                }
            }
        }
    }

    void SpawnMaterial(int _spawnPointIndex)
    {
        int choice = Random.Range(0, 3);
        bool foundValidMaterial = false;
        while (!foundValidMaterial)
        {
            switch (choice)
            {
                case 0:
                    if (bioMaterialsToSpawn > 0)
                    {
                        foundValidMaterial = true;
                    }
                    break;
                case 1:
                    if (archMaterialsToSpawn > 0)
                    {
                        foundValidMaterial = true;
                    }
                    break;
                case 2:
                    if (geoMaterialsToSpawn > 0)
                    {
                        foundValidMaterial = true;
                    }
                    break;
            }
            if (foundValidMaterial)
            {
                break;
            }
            if (bioMaterialsToSpawn == 0 && archMaterialsToSpawn == 0 && geoMaterialsToSpawn == 0)
            {
                return;
            }
            choice = Random.Range(0, 3);
        }
        ResearchMaterial researchMat = null;
        switch (choice)
        {
            case 0:
                researchMat = Instantiate(bioResearchMaterialPrefab, spawnPoints[_spawnPointIndex], Quaternion.identity,researchMaterialSpawnParent.transform).GetComponent<ResearchMaterial>();
                researchMat.SetSpawnInfo(_spawnPointIndex);
                bioMaterialsToSpawn--;
                break;
            case 1:
                researchMat = Instantiate(archResearchMaterialPrefab, spawnPoints[_spawnPointIndex], Quaternion.identity, researchMaterialSpawnParent.transform).GetComponent<ResearchMaterial>();
                researchMat.SetSpawnInfo(_spawnPointIndex);
                archMaterialsToSpawn--;
                break;
            case 2:
                researchMat = Instantiate(geoResearchMaterialPrefab, spawnPoints[_spawnPointIndex], Quaternion.identity, researchMaterialSpawnParent.transform).GetComponent<ResearchMaterial>();
                researchMat.SetSpawnInfo(_spawnPointIndex);
                geoMaterialsToSpawn--;
                break;
        }
        spawnPointAvailability[_spawnPointIndex] = false;
    }


    void InitialMaterialSpawning()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            
            int choice = Random.Range(0, 3);
            bool foundValidMaterial = false;
            while (!foundValidMaterial)
            {
                switch (choice)
                {
                    case 0:
                        if (bioMaterialsToSpawn > 0)
                        {
                            foundValidMaterial = true;
                        }
                        break;
                    case 1:
                        if (archMaterialsToSpawn > 0)
                        {
                            foundValidMaterial = true;
                        }
                        break;
                    case 2:
                        if (geoMaterialsToSpawn > 0)
                        {
                            foundValidMaterial = true;
                        }
                        break;
                }
                if (foundValidMaterial)
                {
                    break;
                }
                if (bioMaterialsToSpawn == 0 && archMaterialsToSpawn == 0 && geoMaterialsToSpawn == 0)
                {
                    return;
                }
                choice = Random.Range(0, 3);
            }
            ResearchMaterial researchMat = null;
            switch (choice)
            {
                case 0:
                    researchMat = Instantiate(bioResearchMaterialPrefab, spawnPoints[i], Quaternion.identity, researchMaterialSpawnParent.transform).GetComponent<ResearchMaterial>();
                    researchMat.SetSpawnInfo(i);
                    bioMaterialsToSpawn--;
                    break;
                case 1:
                    researchMat = Instantiate(archResearchMaterialPrefab, spawnPoints[i], Quaternion.identity, researchMaterialSpawnParent.transform).GetComponent<ResearchMaterial>();
                    researchMat.SetSpawnInfo(i);
                    archMaterialsToSpawn--;
                    break;
                case 2:
                    researchMat = Instantiate(geoResearchMaterialPrefab, spawnPoints[i], Quaternion.identity, researchMaterialSpawnParent.transform).GetComponent<ResearchMaterial>();
                    researchMat.SetSpawnInfo(i);
                    geoMaterialsToSpawn--;
                    break;
            }
            spawnPointAvailability[i] = false;





        }
    }

    //register that a spawn point is now avaiable
    public void MaterialCollected(int _index, ResearchTypes type)
    {
        if (isEnabled)
        {
            spawnPointAvailability[_index] = true;
            if (type == ResearchTypes.Archeology)
            {
                archMaterialsToSpawn++;
            }
            else if (type == ResearchTypes.Biology)
            {
                bioMaterialsToSpawn++;
            }
            else
            {
                geoMaterialsToSpawn++;
            }
            spawnQueue.Add(new SpawnNode(Time.time - GameManager.instance.worldStartTime, _index));
        }
    }



    //Generate uniformally distributed spawn points around the planet using the Fibonacci sphere algorithm
    void GenerateSpawnPoints()
    {
        
        float phi = Mathf.PI * (3 - Mathf.Sqrt(5)); 

        for (int i = 0; i < maxMaterials; i++)
        {
            float y = 1f - (i / (float)(maxMaterials - 1)) * 2f;
           
            float beforeSqrt = 1f - y * y;
            float radius = Mathf.Sqrt(1f - y * y);
            float theta = phi * i;
            float x = Mathf.Cos(theta) * radius;
            float z = Mathf.Sin(theta) * radius;

            spawnPoints.Add(new Vector3(x, y, z) * (planetRadius + 3f));
            spawnPointAvailability.Add(true);
        }
    }

    //called when game ends
    public void Disable()
    {
        isEnabled = false;
    }





}
