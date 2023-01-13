using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SideSelectionButtons : MonoBehaviour
{
    public GameObject otherButton;
    public void FirstMovePicked()
    {
        PlayerScript.Instance.SetSide(TalesOfTribute.PlayerEnum.PLAYER1);
        TalesOfTributeAI.Instance.SetSide(TalesOfTribute.PlayerEnum.PLAYER2);
        GetComponent<Image>().fillCenter = true;
        GetComponent<Image>().color = Color.green;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
        otherButton.GetComponent<Image>().fillCenter = false;
        otherButton.GetComponent<Image>().color = Color.white;
        otherButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        MainMenuScript.SideSelected = true;
    }

    public void SecondMovePicked()
    {
        PlayerScript.Instance.SetSide(TalesOfTribute.PlayerEnum.PLAYER2);
        TalesOfTributeAI.Instance.SetSide(TalesOfTribute.PlayerEnum.PLAYER1);
        GetComponent<Image>().fillCenter = true;
        GetComponent<Image>().color = Color.green;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
        otherButton.GetComponent<Image>().fillCenter = false;
        otherButton.GetComponent<Image>().color = Color.white;
        otherButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        MainMenuScript.SideSelected = true;
    }
}
