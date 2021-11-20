using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoverController : MonoBehaviour
{
    Rigidbody rb;

    private WeaponEquipment weapon;
    public Transform weaponTransform;
    public ParticleSystem weaponFx;

    private bool hasWeapon = false;
    private bool enter = false;

    [Header("Movement")]
    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    public float moveSpeed = 2f;
    public float rotationSpeed = 2f;
    [Range(0.0f,0.6f)]
    public float movementSmoothTime = 0.15f;
    

    [Header("Research")]
    //All research materials have the same mask
    public LayerMask researchMaterialMask;
    public float researchRange = 20f; //TODO: use the rover's equipment range
    public float materialScanTime = 2f; //TODO: use the rover's equipment scan time
    //private bool isCollectingSample = false;
    private int sampleCollectionsLeft = 0;
    private bool isTransmittingData = false;

    [Header("UI")]
    public Text actionText;
    public GameObject scannerObject;
    public Transform scanTransform;
    public float scanSpeed;
    private float scanRange = 0;
    public float scanMaxRange;
    public GameObject transmitterProgressBar = null;
    public Slider transmitterSlider = null;


    [Header("AI")]
    public float aiViewDistance = 15f;
    public float aiShootMaxRange = 5f;
    public LayerMask landingPodLayerMask;
    public LayerMask alienLayerMask;
    public bool isAi = false;

    private bool isEnabled = true;

    [Header("WheelAnimation")]
    public float wheelMaxAngle;
    public float wheelMinAngle;

    public Quaternion frontRightMaxRotation;
    public Quaternion frontRightMinRotation;
    public Quaternion frontRightBaseRotation;
    public Quaternion frontLeftMaxRotation;
    public Quaternion frontLeftMinRotation;
    public Quaternion frontLeftBaseRotation;
    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform[] WheelTransforms;
    

    // Start is called before the first frame update
    void Start()
    {   
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();

        /* 
            Checks if the rover has WeaponEquipment, if so it references the equipment and
            adds the weapon transfrom to the equipment.
            Otherwise, hasWeapon remains false and the contorls have not impact
         */
        int weaponIndex = PlayerManager.instance.GetWeaponEquipment();
        if (weaponIndex != -1)
        {
            weapon = (WeaponEquipment)PlayerManager.instance.equipment[weaponIndex];
            weapon.transform = weaponTransform;
            hasWeapon = true;
        }
        GetComponent<AudioSource>().volume *= AppManager.instance.fxVolume;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnabled) { return; }
        if (!isAi)
        {
            //rotation
            transform.Rotate(Vector3.up * Input.GetAxisRaw("Horizontal") * Time.deltaTime * rotationSpeed);
            //if (Input.GetAxis("Horizontal") > 0)
            //{
            //    frontRightWheelTransform.localRotation = Quaternion.Slerp(frontRightMinRotation, frontRightMaxRotation, Input.GetAxis("Horizontal"));
            //    frontLeftWheelTransform.localRotation = Quaternion.Slerp(frontLeftMaxRotation, frontLeftMinRotation, Input.GetAxis("Horizontal"));
            //}
            //else if (Input.GetAxis("Horizontal") < 0)
            //{
            //    frontRightWheelTransform.localRotation = Quaternion.Slerp(frontRightMaxRotation, frontRightMinRotation, Input.GetAxis("Horizontal"));
            //    frontLeftWheelTransform.localRotation = Quaternion.Slerp(frontLeftMinRotation, frontLeftMaxRotation, Input.GetAxis("Horizontal"));
            //}
            //else
            //{
            //    frontRightWheelTransform.localRotation = frontRightBaseRotation;
            //    frontLeftWheelTransform.localRotation = frontLeftBaseRotation;
            //}
            //frontRightWheelTransform.localRotation = Quaternion.Slerp(frontRightMinRotation,frontRightMaxRotation, Input.GetAxis("Horizontal"));
            //frontLeftWheelTransform.localRotation = Quaternion.Slerp(frontLeftMinRotation, frontLeftMaxRotation, Input.GetAxis("Horizontal"));
            if (Input.GetAxisRaw("Horizontal") == 1f)
            {
                frontRightWheelTransform.localRotation = Quaternion.Slerp(frontRightWheelTransform.localRotation, frontRightMaxRotation, Time.deltaTime * 20f);
                frontLeftWheelTransform.localRotation = Quaternion.Slerp(frontLeftWheelTransform.localRotation, frontLeftMinRotation, Time.deltaTime * 20f);

            }
            else if (Input.GetAxisRaw("Horizontal") == -1f)
            {
                //frontRightWheelTransform.localRotation = frontRightMinRotation;
                //frontLeftWheelTransform.localRotation = frontLeftMaxRotation;
                frontRightWheelTransform.localRotation = Quaternion.Slerp(frontRightWheelTransform.localRotation, frontRightMinRotation, Time.deltaTime * 20f);
                frontLeftWheelTransform.localRotation = Quaternion.Slerp(frontLeftWheelTransform.localRotation, frontLeftMaxRotation, Time.deltaTime * 20f);
            }
            else
            {
                frontRightWheelTransform.localRotation = Quaternion.Slerp(frontRightWheelTransform.localRotation, frontRightBaseRotation, Time.deltaTime * 20f);
                frontLeftWheelTransform.localRotation = Quaternion.Slerp(frontLeftWheelTransform.localRotation, frontLeftBaseRotation, Time.deltaTime * 20f);
                //frontRightWheelTransform.localRotation = frontRightBaseRotation;
                //frontLeftWheelTransform.localRotation = frontLeftBaseRotation;
            }

            //movement
            Vector3 moveDir = new Vector3(0, 0, Input.GetAxisRaw("Vertical")).normalized;
            Vector3 targetMoveAmount = moveDir * moveSpeed;
            moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, movementSmoothTime);

            if (Input.GetAxisRaw("Vertical") == 1)
            {
                foreach (Transform wheel in WheelTransforms)
                {
                    wheel.RotateAround(wheel.position, transform.right, 5f);
                }
            }
            else if (Input.GetAxisRaw("Vertical") == -1)
            {
                foreach (Transform wheel in WheelTransforms)
                {
                    wheel.RotateAround(wheel.position, transform.right, -5f);
                }
            }

            //research material interaction
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (!isTransmittingData)
                {
                    CheckForNearbyResearchMaterial();
                }
                else
                {
                    Debug.Log("Can't scan while transmitting data");
                }
            }

            //transmit research data to mission control
            if (Input.GetKeyDown(KeyCode.E))
            {
                TransmitterEquipment transmitter = (TransmitterEquipment)PlayerManager.instance.equipment[1];
                transmitter.TransmitResearchData();

            }

            // Weapon functionality
            if (hasWeapon)
            {
                weapon.Rotate();

                if (Input.GetMouseButton(0))
                {
                    if (enter == false)
                    {
                        StartCoroutine(timer(weapon.fireRate));
                        weaponFx.Play();
                        weapon.Shoot();
                        
                    }
                }
            }



        }
        else
        {
            // check for landing pods if there is at least one landing pod on the planet
            if (PlayerManager.instance.GetComponent<LandingPodSpawner>().currentPods.Count > 0)
            {
                Collider[] nearbyLandingPods = Physics.OverlapSphere(transform.position, 3f, landingPodLayerMask);
                //if we are not near a landing pod, then go towards one
                if (nearbyLandingPods.Length == 0)
                {
                    if (PlayerManager.instance.nearbyEnergyArrowAngle != -10000f)
                    {
                        if (PlayerManager.instance.nearbyEnergyArrowAngle < -5f)
                        {
                            //turn right
                            transform.Rotate(Vector3.up * 0.8f * Time.deltaTime * rotationSpeed);
                        }
                        else if (PlayerManager.instance.nearbyEnergyArrowAngle > 5f)
                        {
                            //turn left
                            transform.Rotate(Vector3.up * -0.8f * Time.deltaTime * rotationSpeed);
                        }
                    }
                    //move forward
                    Vector3 moveDir = new Vector3(0, 0, 1f).normalized;
                    Vector3 targetMoveAmount = moveDir * moveSpeed;
                    moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, movementSmoothTime);
                }
                else
                {
                    //don't move since we're refueling at landing pod
                    moveAmount = Vector3.zero;
                }
            }
            else
            {
                //start scanning to try to collect research materials
                if (!isTransmittingData)
                {
                    CheckForNearbyResearchMaterial();
                }
                //move forward
                Vector3 moveDir = new Vector3(0, 0, 1f).normalized;
                Vector3 targetMoveAmount = moveDir * moveSpeed;
                moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, movementSmoothTime);
            }
            if (hasWeapon)
            {
                Collider[] nearbyAliens = Physics.OverlapSphere(transform.position, aiShootMaxRange, alienLayerMask);
                if (nearbyAliens.Length > 0)
                {
                    
                    Vector3 directionToAlien = (nearbyAliens[0].transform.position - weaponTransform.position).normalized;
                    Vector3 projectedDirection = Vector3.ProjectOnPlane(directionToAlien, weaponTransform.forward).normalized;
                    float angleDifference = Vector3.Angle(-weaponTransform.up, projectedDirection);
                    if (angleDifference > 2f)
                    {
                        //rotate weapon
                        weapon.Rotate(1f);
                    }
                    else if (angleDifference < -2f)
                    {
                        //rotate weapon
                        weapon.Rotate(-1f);
                    }
                    else
                    {
                        //shoot
                        if (enter == false)
                        {
                            StartCoroutine(timer(weapon.fireRate));
                            weaponFx.Play();
                            weapon.Shoot();
                        }
                    }
                      


                }
            }
        }

        //if scanning, update scan effect
        if (sampleCollectionsLeft > 0)
        {
            scanRange += scanSpeed * Time.deltaTime;
            if (scanRange > scanMaxRange)
            {
                scanRange = 0f;
            }
            scanTransform.localScale = new Vector3(scanRange, scanRange);
        }

    }

    private void FixedUpdate()
    {
        if (!isEnabled) { return; }
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
        
    }

    private void CheckForNearbyResearchMaterial()
    {
        if (sampleCollectionsLeft == 0) // don't check if we are already scanning something
        {
            
            //make a list of the research equipment that we have
            List<ResearchEquipment> researchEquipmentArray = new List<ResearchEquipment>();
            foreach (Equipment equipment in PlayerManager.instance.equipment)
            {
                if (equipment is ResearchEquipment)
                {
                    ResearchEquipment researchEquipment = (ResearchEquipment)equipment;
                    researchEquipmentArray.Add(researchEquipment);
                }
            }
            if (researchEquipmentArray.Count == 0)
            {
                Debug.Log("No research equipment available.");
                return;
            }
            
            scanMaxRange = 0f;
            //start scanning using each different type of research equipment that we have
            foreach (ResearchEquipment e in researchEquipmentArray)
            {
                float range = e.range;
                if (.5f * range > scanMaxRange)
                {
                    scanMaxRange = .5f * range;
                }
                float scanTime = e.scanTime;
                Collider[] nearbyMaterials = Physics.OverlapSphere(transform.position, range, researchMaterialMask);
                
                if (nearbyMaterials.Length > 0)
                {
                    //check if a valid research material is within range
                    for (int i = 0; i < nearbyMaterials.Length; i++)
                    {
                        ResearchMaterial researchMaterial = nearbyMaterials[i].GetComponent<ResearchMaterial>();
                        if (researchMaterial.GetResearchType() == e.type)
                        {
                            
                            //scan the research material
                            StartCoroutine(MaterialScanRoutine(scanTime, researchMaterial));
                            break;
                        }
                    }
                }

            }

        }
        
    }

    private IEnumerator MaterialScanRoutine(float _materialScanTime,ResearchMaterial _researchMaterial)
    {
        //isCollectingSample = true;
        scannerObject.SetActive(true);
        sampleCollectionsLeft++;
        actionText.text = "Scanning";
        yield return new WaitForSeconds(_materialScanTime);
        if (!isEnabled) { yield break; }
        actionText.text = "";
        //make sure that we are still within range of the material
        if ((_researchMaterial.transform.position - transform.position).magnitude <= researchRange)
        {
            
            if (!PlayerManager.instance.CollectResearchMaterial(_researchMaterial))
            {
                if (actionText.text == "")
                {
                    actionText.text = "Full capacity";
                }
            }
            
        }
        else
        {
            //Debug.Log("No longer in range of research material");
        }
        scannerObject.SetActive(false);
        sampleCollectionsLeft--;
        //isCollectingSample = false;
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, researchRange);
    }

    private IEnumerator timer(float timeDelay)
    {
        enter = true;
        yield return new WaitForSeconds(timeDelay);
        if (!isEnabled) { yield break; }
        enter = false;
    }

    public void SetActionText(string text)
    {
        actionText.text = text;
    }

    public void SetIsTransmitting(bool _transmitting)
    {
        isTransmittingData = _transmitting;
    }

    public void Disable()
    {
        isEnabled = false;
        if (PlayerManager.instance.health <= 0)
        {
            //TODO: blowup?
        }
        
    }

    public void SetAI(bool useAI)
    {
        isAi = useAI;
    }
}
