using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiscButtonHandler : MonoBehaviour
{

    public GameObject SettingsPanel;
    public void CloseApp()
    {
        Application.Quit();
    }
    public void ConfirmPatronChoices()
    {
        FindObjectOfType<PatronSelectionScript>().ProceedGame();
    }

    public void RefreshScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void ChangeResolution1280x720()
    {
        Screen.SetResolution(1280, 720, false);
        SettingsPanel.SetActive(false);
    }

    public void ChangeResolution1600x900()
    {
        Screen.SetResolution(1600, 900, false);
        SettingsPanel.SetActive(false);
    }

    public void ChangeResolution1920x1080()
    {
        Screen.SetResolution(1920, 1080, false);
        SettingsPanel.SetActive(false);
    }

    public void ChangeResolution2560x1440()
    {
        Screen.SetResolution(2560, 1440, false);
        SettingsPanel.SetActive(false);
    }

    public void SetFullScreen()
    {
        Screen.fullScreen = true;
        SettingsPanel.SetActive(false);
    }

    public void SetWindowed()
    {
        Screen.fullScreen = false;
        SettingsPanel.SetActive(false);
    }

    public void ShowSettings()
    {
        SettingsPanel.SetActive(!SettingsPanel.activeInHierarchy);
    }

    

}
