using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeologyMaterial : ResearchMaterial
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if (spawnPointIndex != -1)
        {
            if (GameManager.instance == null) { return; }
            GameManager.instance.GetComponent<ResearchMaterialSpawner>().MaterialCollected(spawnPointIndex, ResearchTypes.Geology);
        }
    }
}
