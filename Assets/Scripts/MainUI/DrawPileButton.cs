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
        var serializer = GameManager.Board.GetSerializer();
        if (serializer.CurrentPlayer.PlayerID == playerId)
            CardShowUI.GetComponent<CardShowUIScript>().cards = serializer.CurrentPlayer.DrawPile.ToArray();
        else
            CardShowUI.GetComponent<CardShowUIScript>().cards = serializer.EnemyPlayer.DrawPile.ToArray();
        GameManager.isUIActive = true;
        CardShowUI.SetActive(true);
    }
}
