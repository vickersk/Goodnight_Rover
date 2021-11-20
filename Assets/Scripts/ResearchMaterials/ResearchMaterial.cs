using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchMaterial : MonoBehaviour
{
    [SerializeField] //this makes a private or protected variable visible in the inspector
    protected int worth;

    [SerializeField]
    protected ResearchTypes researchType;

    protected int spawnPointIndex = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Research material tells us the index of the spawn point we spawned at. We relay back this info when we are destroyed.
    public void SetSpawnInfo(int _index)
    {
        spawnPointIndex = _index;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ResearchTypes GetResearchType() { return researchType; }

    public int GetWorth() { return worth; }



}
