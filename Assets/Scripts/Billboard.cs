using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    //This script makes the attached canvas always point towards the camera


    private Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindObjectOfType<Camera>().transform;
    }

    // LateUpdate is called after update
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
