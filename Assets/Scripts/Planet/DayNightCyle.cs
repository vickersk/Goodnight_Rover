using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightCyle : MonoBehaviour
{

    public RectTransform dayNightCycleRect;
    public RectTransform background;
    public RectTransform nightStartHand;
    public RectTransform nightEndHand;
    public Text dayText;
    private Transform roverTransform;
    public Light sun;
    public Gradient gradient;

    private GameObject energyBar;
    private GameObject healthBar;
    private GameObject fundingText;
    private GameObject directions;
    private GameObject dayNightCycle;
    private GameObject arrow;


    //How many seconds for a whole day night cycle
    public float dayTimeScale = 30f;



    public float nightStartAngle = 90f;
    public float nightLightIntensity = .01f;

    //How many degrees to do the sunset/sunrise
    public float sunsetDuration = 30f;


    public int currentDay = 0;
    private float currentRotationAngle = 0f;
    private float lastDayChangeTime = 0f;
    private Vector3 gameLocation = new Vector3(11f, 366f, 0);
    private Vector3 menuLocation = new Vector3(50f, 294f, 0);
    
    private bool isEnabled = true;
    private bool isBlackoutPeriod = false;



    // Start is called before the first frame update
    void Start()
    {
        nightStartHand.eulerAngles = new Vector3(0f, 0f, 360 - nightStartAngle);
        nightEndHand.eulerAngles = new Vector3(0f, 0f, 0f);
        roverTransform = FindObjectOfType<RoverController>().transform;
        energyBar = GameObject.Find("EnergyBar");
        healthBar = GameObject.Find("HealthBar");
        fundingText = GameObject.Find("FundingText");
        dayNightCycle = GameObject.Find("DayNightCycle");
        arrow = GameObject.Find("FuelArrow");
        directions = GameObject.Find("Directions (Temporary)");


    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
        {
            //update current day
            if (Time.time - GameManager.instance.worldStartTime - lastDayChangeTime >= dayTimeScale)
            {
                if (PlayerManager.instance != null)// && !PlayerManager.instance.isDead)
                {
                    currentDay++;
                    dayText.text = String.Format("Day:{0}", currentDay);
                    lastDayChangeTime = Time.time - GameManager.instance.worldStartTime;
                }
            }
            currentRotationAngle = (Time.time - GameManager.instance.worldStartTime - lastDayChangeTime) / dayTimeScale * 360f;

            if (currentRotationAngle >= 0 && currentRotationAngle < sunsetDuration)
            {

                sun.intensity = Mathf.Lerp(nightLightIntensity, 1f, currentRotationAngle / sunsetDuration);
                sun.color = gradient.Evaluate(currentRotationAngle / sunsetDuration);
                if (isBlackoutPeriod)
                {
                    roverTransform.GetComponent<RoverController>().SetAI(false);
                    UpgradeUI.instance.SetUpgradeMenu(false);
                    GameManager.instance.UpdateFundingText();
                    energyBar.SetActive(true);
                    healthBar.SetActive(true);
                    fundingText.SetActive(true);
                    arrow.SetActive(true);
                    directions.SetActive(true);
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    InventoryUI.instance.gameObject.SetActive(true);
                    dayNightCycleRect.anchoredPosition = new Vector2(10, -10);
                    isBlackoutPeriod = false;
                }

            }
            else if (currentRotationAngle < nightStartAngle && currentRotationAngle > nightStartAngle - sunsetDuration)
            {

                sun.intensity = Mathf.Lerp(nightLightIntensity, 1f, (nightStartAngle - currentRotationAngle) / sunsetDuration);
                sun.color = gradient.Evaluate((nightStartAngle - currentRotationAngle) / sunsetDuration);
                if (isBlackoutPeriod)
                {
                    roverTransform.GetComponent<RoverController>().SetAI(false);
                    UpgradeUI.instance.SetUpgradeMenu(false);
                    GameManager.instance.UpdateFundingText();
                    energyBar.SetActive(true);
                    healthBar.SetActive(true);
                    fundingText.SetActive(true);
                    arrow.SetActive(true);
                    directions.SetActive(true);
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    InventoryUI.instance.gameObject.SetActive(true);
                    dayNightCycleRect.anchoredPosition = new Vector2(10, -10);
                    isBlackoutPeriod = false;
                }
            }
            else if (currentRotationAngle >= nightStartAngle && currentRotationAngle < 360f)
            {
                //blackout period
                if (!isBlackoutPeriod)
                {

                    sun.intensity = .001f;
                    roverTransform.GetComponent<RoverController>().SetAI(true);
                    UpgradeUI.instance.SetUpgradeMenu(true);
                    energyBar.SetActive(false);
                    healthBar.SetActive(false);
                    fundingText.SetActive(false);
                    directions.SetActive(false);
                    arrow.SetActive(false);
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    InventoryUI.instance.gameObject.SetActive(false);
                    dayNightCycleRect.anchoredPosition = new Vector2(50, -70);
                    isBlackoutPeriod = true;
                }
            }




            sun.transform.rotation = Quaternion.LookRotation(-roverTransform.up);

            //rotate the ui
            background.eulerAngles = new Vector3(0f, 0f, currentRotationAngle);
        }
    }

    public void Disable()
    {
        isEnabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
