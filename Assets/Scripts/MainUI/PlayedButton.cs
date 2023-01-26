using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TalesOfTribute;

public class PlayedButton : MonoBehaviour
{
    public GameObject CardShowUI;
    public bool isBot;

    public void OnClick()
    {
        var serializer = GameManager.Board.GetFullGameState();
        var playerId = isBot ? TalesOfTributeAI.Instance.botID : PlayerScript.Instance.playerID;
        if (serializer.CurrentPlayer.PlayerID == playerId)
            CardShowUI.GetComponent<CardShowUIScript>().cards = serializer.CurrentPlayer.Played.ToArray();
        else
            CardShowUI.GetComponent<CardShowUIScript>().cards = serializer.EnemyPlayer.Played.ToArray();
        CardShowUI.GetComponent<CardShowUIScript>().title.SetText("Played Pile");
        CardShowUI.SetActive(true);
    }
}
