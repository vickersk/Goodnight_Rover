using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationUI : MonoBehaviour
{
    public Text wAmountText;
    public Text gAmountText;
    public Text bAmountText;
    public Text aAmountText;
    public Text availableSlots;
    public Text currentFunding;
    public Button removeButton;
    public ButtonHandler addButton;


    public static CustomizationUI instance;
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
        UpdateUI();
        removeButton.gameObject.SetActive(false);
        addButton.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        if (CustomizationManager.instance.numEquipment[0] == 0)
        {
            removeButton.gameObject.SetActive(false);
            addButton.gameObject.SetActive(true);
        }
        else if (CustomizationManager.instance.numEquipment[0] > 0 &&
                CustomizationManager.instance.numEquipment[0] < CustomizationManager.instance.maxEquipment[0])
        {
            removeButton.gameObject.SetActive(true);
            addButton.gameObject.SetActive(true);
        }
        else
        {
            removeButton.gameObject.SetActive(true);
            addButton.gameObject.SetActive(false);
        }
    }

    private void UpdateUI()
    {
        wAmountText.text = String.Format(
            "Amount:\n{0}/{1}", CustomizationManager.instance.numEquipment[0],
            CustomizationManager.instance.maxEquipment[0]);
        gAmountText.text = String.Format(
            "Amount:\n{0}/{1}", CustomizationManager.instance.numEquipment[1],
            CustomizationManager.instance.maxEquipment[1]);
        bAmountText.text = String.Format(
            "Amount:\n{0}/{1}", CustomizationManager.instance.numEquipment[2],
            CustomizationManager.instance.maxEquipment[2]);
        aAmountText.text = String.Format(
            "Amount:\n{0}/{1}", CustomizationManager.instance.numEquipment[3],
            CustomizationManager.instance.maxEquipment[3]);
        availableSlots.text = String.Format(
            "Available Slots:\n{0}/{1}", CustomizationManager.instance.openSlots,
            CustomizationManager.instance.totalSlots);

        currentFunding.text = String.Format("Current Funding:\n${0}", AppManager.instance.funding);
    }
}
