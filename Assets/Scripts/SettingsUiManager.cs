using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsUiManager : MonoBehaviour
{

    public InputField musicVolumeInputField;
    public InputField fxVolumeInputField;
    public InputField mouseSensitivityInputField;


    private void Start()
    {
        musicVolumeInputField.text = FindObjectOfType<AppManager>().musicVolume.ToString();
        fxVolumeInputField.text = FindObjectOfType<AppManager>().fxVolume.ToString();
        mouseSensitivityInputField.text = FindObjectOfType<AppManager>().mouseSensitivity.ToString();
    }
    public void Back()
    {
        float musicVolume;
        if (float.TryParse(musicVolumeInputField.text,out musicVolume))
        {
            musicVolume = Mathf.Clamp(musicVolume, 0f, 10f);
            FindObjectOfType<AppManager>().musicVolume = musicVolume;
        }
        float fxVolume;
        if (float.TryParse(fxVolumeInputField.text, out fxVolume))
        {
            fxVolume = Mathf.Clamp(fxVolume, 0f, 10f);
            FindObjectOfType<AppManager>().fxVolume = fxVolume;
        }
        float mouseSensitivity;
        if (float.TryParse(mouseSensitivityInputField.text, out mouseSensitivity))
        {
            mouseSensitivity = Mathf.Clamp(mouseSensitivity, 1f, 10f);
            FindObjectOfType<AppManager>().mouseSensitivity = mouseSensitivity;
        }
        FindObjectOfType<AudioManager>().UpdateVolume();
        FindObjectOfType<AudioManager>().Play("Button Click");
        SceneManager.LoadScene("StartMenu");
    }
}
