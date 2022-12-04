using System.Collections;
using System.Collections.Generic;
using TalesOfTribute;
using UnityEngine;

public class DrawPileButton : MonoBehaviour
{
    public GameObject CardShowUI;
    public PlayerEnum playerId;

    public void OnClick()
    {
        CardShowUI.GetComponent<CardShowUIScript>().cards = GameManager.Board.GetDrawPile(playerId).ToArray();
        GameManager.isUIActive = true;
        CardShowUI.SetActive(true);
    }
}
