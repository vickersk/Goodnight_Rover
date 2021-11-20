using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    private const int LANDING_POD_COST = 1000;
    private bool selectToggle = true;
    private Text buttonText;
    private ButtonHandler removeButton;

    public void Purchase()
    {
        FindObjectOfType<AudioManager>().Play("Button Click");
        int index = this.gameObject.transform.parent.gameObject.transform.GetSiblingIndex();
        Upgrade upgrade = UpgradeManager.instance.upgrades[index];

        if (upgrade.cost <= AppManager.instance.funding)
        {
            AppManager.instance.SetFunding(AppManager.instance.funding - upgrade.cost);
            PlayerManager.instance.equipment[index].AddUpgrade(upgrade);
            UpgradeManager.instance.UpdateUpgrades(index);
            UpgradeUI.instance.UpdateUI();

        }
    }

    public void PurchaseDropPod()
    {
        FindObjectOfType<AudioManager>().Play("Button Click");
        if (LANDING_POD_COST <= AppManager.instance.funding)
        {
            AppManager.instance.SetFunding(AppManager.instance.funding - LANDING_POD_COST);
            LandingPodSpawner.instance.SpawnPod();
        }
    }

    public void SelectEquipment()
    {
        FindObjectOfType<AudioManager>().Play("Button Click");
        int index = this.gameObject.transform.parent.gameObject.transform.GetSiblingIndex();
        buttonText = this.gameObject.transform.GetChild(0).GetComponent<Text>();


        if (selectToggle == true)
        {
            if (CustomizationManager.instance.openSlots != 0)
            {
                CustomizationManager.instance.AddEquipment(index);
                selectToggle = false;
                buttonText.text = "Remove";
            }
        }
        else
        {
            CustomizationManager.instance.RemoveEquipment(index);
            selectToggle = true;
            buttonText.text = "Add";
        }
    }

    public void AddWeaponryEquipment()
    {
        FindObjectOfType<AudioManager>().Play("Button Click");
        int index = this.gameObject.transform.parent.gameObject.transform.GetSiblingIndex();

        CustomizationManager.instance.AddEquipment(index);

    }

    public void RemoveWeaponryEquipment()
    {
        FindObjectOfType<AudioManager>().Play("Button Click");
        int index = this.gameObject.transform.parent.gameObject.transform.GetSiblingIndex();

        CustomizationManager.instance.RemoveEquipment(index);
    }

    public void Launch()
    {
        FindObjectOfType<AudioManager>().Play("Button Click");
        SceneManager.LoadScene("Main");
    }
}
