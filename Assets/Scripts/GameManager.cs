using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    public Text fundingText;

    //Singleton implementation
    public static GameManager instance;

    public float worldStartTime = 0f;
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
    
    void Start()
    {

        
        
        fundingText.text = String.Format("Funding: ${0}", AppManager.instance.funding);
        worldStartTime = Time.time;

    }


    public void ReceiveResearchData(int _researchWorth)
    {
        Debug.Log(String.Format("Mission Control: Received research worth {0} dollars.", _researchWorth));
        AppManager.instance.SetFunding(AppManager.instance.funding + _researchWorth);
        UpdateFundingText();
    }

    public void UpdateFundingText()
    {
        fundingText.text = String.Format("Funding: ${0}", AppManager.instance.funding);
    }


    private void Update()
    {

    }


    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void RestartGame()
    {
        Destroy(FindObjectOfType<CustomizationManager>().gameObject);
        SceneManager.LoadScene("CustomizationMenu");
    }
}
