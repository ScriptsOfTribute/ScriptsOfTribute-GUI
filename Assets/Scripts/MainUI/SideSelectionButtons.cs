using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideSelectionButtons : MonoBehaviour
{
    public GameObject otherButton;
    public void FirstMovePicked()
    {
        PlayerScript.Instance.SetSide(TalesOfTribute.PlayerEnum.PLAYER1);
        TalesOfTributeAI.Instance.SetSide(TalesOfTribute.PlayerEnum.PLAYER2);
        this.GetComponent<Image>().color = Color.magenta;
        otherButton.GetComponent<Image>().color = Color.grey;
        MainMenuScript.SideSelected = true;
    }

    public void SecondMovePicked()
    {
        PlayerScript.Instance.SetSide(TalesOfTribute.PlayerEnum.PLAYER2);
        TalesOfTributeAI.Instance.SetSide(TalesOfTribute.PlayerEnum.PLAYER1);
        this.GetComponent<Image>().color = Color.magenta;
        otherButton.GetComponent<Image>().color = Color.grey;
        MainMenuScript.SideSelected = true;
    }
}
