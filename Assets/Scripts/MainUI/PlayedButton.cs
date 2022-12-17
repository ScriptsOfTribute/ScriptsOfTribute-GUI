using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TalesOfTribute;

public class PlayedButton : MonoBehaviour
{
    public GameObject CardShowUI;
    public PlayerEnum playerId;

    public void OnClick()
    {
        CardShowUI.GetComponent<CardShowUIScript>().cards = GameManager.Board.GetPlayedCards(playerId).ToArray();
        GameManager.isUIActive = true;
        CardShowUI.SetActive(true);
    }
}
