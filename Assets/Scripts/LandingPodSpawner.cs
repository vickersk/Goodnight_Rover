using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingPodSpawner : MonoBehaviour
{

    public List<LandingPod> currentPods = new List<LandingPod>();
    public GameObject landingPodPrefab;
    private GameObject landingPodsParent;
    public int startingPodCount = 1;
    public float maxRoverPodInitialDistance = 30f;
    public float minRoverPodInitialDistance = 10f;

    private Transform planetTransform;
    private float planetRadius;
    private Transform roverTransform;


    private bool isEnabled = true;

    

    public static LandingPodSpawner instance;
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
        planetTransform = FindObjectOfType<PlanetGravity>().transform;
        planetRadius = planetTransform.GetComponent<SphereCollider>().radius * planetTransform.localScale.x;
        roverTransform = FindObjectOfType<RoverController>().transform;
        landingPodsParent = new GameObject("Landing Pods");
        for (int i = 0; i < startingPodCount;i++)
        {
            SpawnPod();
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void SpawnPod()
    {
        if (isEnabled)
        {

            Vector3 planetCenterToLocation = roverTransform.position + Random.onUnitSphere * maxRoverPodInitialDistance;
            Vector3 planetCenterToLocationOnPlanet = planetCenterToLocation.normalized * (planetRadius);
            Vector3 spawnLocation = planetCenterToLocation + (planetCenterToLocationOnPlanet - planetCenterToLocation);
            while (Vector3.SqrMagnitude(spawnLocation - roverTransform.position) <= minRoverPodInitialDistance * minRoverPodInitialDistance)
            {
                planetCenterToLocation = roverTransform.position + Random.onUnitSphere * maxRoverPodInitialDistance;


                planetCenterToLocationOnPlanet = planetCenterToLocation.normalized * (planetRadius);
                spawnLocation = planetCenterToLocation + (planetCenterToLocationOnPlanet - planetCenterToLocation);
            }
            spawnLocation += planetCenterToLocation.normalized * 10f;
            LandingPod pod = Instantiate(landingPodPrefab, spawnLocation, Quaternion.identity, landingPodsParent.transform).GetComponent<LandingPod>();
            pod.Initialize();
            pod.SetSpawnInfo(currentPods.Count);
            currentPods.Add(pod);
        }
    }

    public void PodDestroyed(int _index)
    {
        if (isEnabled)
        {
            currentPods.RemoveAt(_index);
        }
    }

    public void Disable()
    {
        isEnabled = false;
        foreach (LandingPod pod in currentPods)
        {
            //only destroy pod component so pod mesh is still there
            Destroy(pod);
        }
    }
}
