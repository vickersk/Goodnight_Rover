using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class GravityBody : MonoBehaviour
{

    PlanetGravity planetGravity;

    private void Awake()
    {
        planetGravity = GameObject.FindGameObjectWithTag("Planet").GetComponent<PlanetGravity>();
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        planetGravity.Attract(transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
