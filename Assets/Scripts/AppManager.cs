using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    /*
     * This class will be persistent throughout the whole application.
     */

    
    public int INITIAL_FUNDING = 100000;

    public int funding { get; set; }
    

    public static AppManager instance;

    [Range (0.0f,10.0f)]
    public float musicVolume = 5f;
    [Range(0.0f, 10.0f)]
    public float fxVolume = 5f;
    [Range(1f, 10f)]
    public float mouseSensitivity = 2f;

    //longest amount of days a mission has lasted
    private int highScore = 0;

    public bool usePlayerPrefs = true;

    //Singleton implementation
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //make sure that this object is not destroyed when switching between scenes
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (usePlayerPrefs)
        {
            if (PlayerPrefs.GetInt("High_Score", -1) != -1)
            {
                highScore = PlayerPrefs.GetInt("High_Score");
            }
            if (PlayerPrefs.GetInt("Funding", -1) != -1)
            {
                funding = PlayerPrefs.GetInt("Funding");
            }
            else
            {
                funding = INITIAL_FUNDING;
            }
        }
        else
        {
            funding = INITIAL_FUNDING;
        }
        
    }

    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time);
    }


    public void SetHighScore(int _score)
    {
        if (_score > highScore)
        {
            highScore = _score;
            if (usePlayerPrefs)
            {
                PlayerPrefs.SetInt("High_Score", highScore);
            }
        }
    }

    public void SetFunding(int _funding)
    {
        funding = _funding;
        if (usePlayerPrefs)
        {
            
            PlayerPrefs.SetInt("Funding", funding);
        }
    }

    public int GetHighScore() { return highScore; }
}
