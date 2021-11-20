using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpMenuUIManager : MonoBehaviour
{
    public void Back()
    {
        FindObjectOfType<AudioManager>().Play("Button Click");
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("HelpMenu2"))
        {
            SceneManager.LoadScene("HelpMenu");
        }
        else
        {
            SceneManager.LoadScene("StartMenu");
        }
    }

    public void Continue()
    {
        FindObjectOfType<AudioManager>().Play("Button Click");
        SceneManager.LoadScene("HelpMenu2");
    }
}
