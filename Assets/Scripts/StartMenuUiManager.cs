using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuUiManager : MonoBehaviour
{
    public void LoadSettingsMenu()
    {
        FindObjectOfType<AudioManager>().Play("Button Click");
        SceneManager.LoadScene("SettingsMenu");
    }

    public void StartGame()
    {
        FindObjectOfType<AudioManager>().Play("Button Click");
        SceneManager.LoadScene("CustomizationMenu");
    }

    public void QuitApplication()
    {
        FindObjectOfType<AudioManager>().Play("Button Click");
        Application.Quit();
    }

    public void LoadHelpMenu()
    {
        FindObjectOfType<AudioManager>().Play("Button Click");
        SceneManager.LoadScene("HelpMenu");
    }
}
