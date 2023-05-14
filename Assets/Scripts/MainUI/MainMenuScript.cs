using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject PatronSelection;
    public GameObject MainMenu;
    public TextMeshProUGUI CantPlayReason;
    public static bool BotSelected = false;
    public static bool SideSelected = false;

    private void Start()
    {
        BotSelected = false;
        SideSelected = false;
    }
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

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (BotSelected && !SideSelected)
        {
            CantPlayReason.SetText("Pick side");
        }
        else if (!BotSelected && SideSelected)
        {
            CantPlayReason.SetText("Pick AI to play against");
        }
        else if (!BotSelected && !SideSelected)
        {
            CantPlayReason.SetText("Pick side and AI to play against");
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        CantPlayReason.SetText("");
    }

}
