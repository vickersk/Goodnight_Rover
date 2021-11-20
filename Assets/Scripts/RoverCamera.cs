using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverCamera : MonoBehaviour
{
    public Transform roverCloseCameraTransform;
    public Transform roverFarCameraTransform;
    public Transform roverTransform;
    public RoverController rover;
    private Vector3 cameraOffset;

    [Range (0.0f,1.0f)]
    public float smoothFactor = .5f;

    [Range (0.0f,0.8f)]
    public float lookAheadAmount = .2f;

    [Range (0.0f,1.0f)]
    public float currentZoomPercent = 1.0f;

    public float zoomSensitivity = .2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (rover.isAi) { return; }
        currentZoomPercent += Input.mouseScrollDelta.y * zoomSensitivity;
        currentZoomPercent = Mathf.Clamp(currentZoomPercent, 0.0f, 1.0f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPos = Vector3.Lerp(roverFarCameraTransform.position, roverCloseCameraTransform.position, currentZoomPercent);

        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);

        transform.rotation = Quaternion.LookRotation(-roverTransform.up + roverTransform.forward * lookAheadAmount, roverTransform.forward + roverTransform.forward * lookAheadAmount);
    }
}
