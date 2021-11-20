using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransmitterEquipment : Equipment
{
    public float uploadFactor { get; private set; }
    private bool isTransmitting = false;

    public override void Initialize(string name, int cost, int level, float uploadFactor)
    {
        base.Initialize(name, cost, level);
        this.uploadFactor = uploadFactor;
    }


    public override void AddUpgrade(Upgrade upgrade)
    {
        if (upgrade is TransmitterUpgrade)
        {
            upgrades.Add(upgrade);
            TransmitterUpgrade transmitterUpgrade = (TransmitterUpgrade)upgrade;

            level = transmitterUpgrade.level;
            uploadFactor = transmitterUpgrade.uploadMultiplier;
        }
    }

    public void TransmitResearchData()
    {
        if (!isTransmitting)
        {
            if (PlayerManager.instance.pickedUpResearchMaterials.Count > 0)
            {
                isTransmitting = true;
                Debug.Log("Starting transmission...");
                PlayerManager.instance.rover.SetActionText("Transmitting");
                PlayerManager.instance.rover.SetIsTransmitting(true);
                PlayerManager.instance.rover.transmitterProgressBar.SetActive(true);
                StartCoroutine(TransmittionDelayRoutine(uploadFactor));
                
            }
        }
    }

    public void FinishTransmittingData()
    {
        PlayerManager.instance.rover.SetActionText("");
        PlayerManager.instance.rover.SetIsTransmitting(false);
        int totalResearchWorth = 0;
        foreach (ResearchMaterialStruct researchMaterialStruct in PlayerManager.instance.pickedUpResearchMaterials)
        {
            totalResearchWorth += researchMaterialStruct.worth;
        }
        GameManager.instance.ReceiveResearchData(totalResearchWorth);
        PlayerManager.instance.pickedUpResearchMaterials.Clear();
        PlayerManager.instance.rover.transmitterProgressBar.SetActive(false);
        isTransmitting = false;
    }



    private IEnumerator TransmittionDelayRoutine(float _delay)
    {
        
        float time = 0;
        Slider transmitterSlider = PlayerManager.instance.rover.transmitterSlider;
        while (time <= 1f)
        {
            time += Time.deltaTime / _delay;
            transmitterSlider.value = Mathf.Lerp(0f, 1f, time);
            yield return null;
        }
        FinishTransmittingData();
    }

}
