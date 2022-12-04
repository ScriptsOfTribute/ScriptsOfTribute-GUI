using System.Collections;
using System.Collections.Generic;
using TalesOfTribute;
using UnityEngine;

public class CooldownPileButton : MonoBehaviour
{
    public GameObject CardShowUI;
    public PlayerEnum playerId;

    public void OnClick()
    {
        CardShowUI.GetComponent<CardShowUIScript>().cards = GameManager.Board.GetCooldownPile(playerId).ToArray();
        GameManager.isUIActive = true;
        CardShowUI.SetActive(true);
    }
}
