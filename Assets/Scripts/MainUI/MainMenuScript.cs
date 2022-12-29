using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public GameObject PatronSelection;
    public GameObject MainMenu;
    public static bool BotSelected = false;
    public static bool SideSelected = false;

    private void Update()
    {
        if (BotSelected && SideSelected)
            GetComponent<Button>().interactable = true;
    }
    public void Play()
    {
        PatronSelection.SetActive(true);
        MainMenu.SetActive(false);
    }

}
