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
        var serializer = GameManager.Board.GetSerializer();
        if (serializer.CurrentPlayer.PlayerID == playerId)
            CardShowUI.GetComponent<CardShowUIScript>().cards = serializer.CurrentPlayer.Played.ToArray();
        else
            CardShowUI.GetComponent<CardShowUIScript>().cards = serializer.EnemyPlayer.Played.ToArray();
        GameManager.isUIActive = true;
        CardShowUI.SetActive(true);
    }
}
