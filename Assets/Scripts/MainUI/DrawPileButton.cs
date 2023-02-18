using System.Collections;
using System.Collections.Generic;
using ScriptsOfTribute;
using UnityEngine;

public class DrawPileButton : MonoBehaviour
{
    public GameObject CardShowUI;
    public bool isBot;

    public void OnClick()
    {
        var serializer = GameManager.Board.GetFullGameState();
        var playerId = isBot ? ScriptsOfTributeAI.Instance.botID : PlayerScript.Instance.playerID;
        if (serializer.CurrentPlayer.PlayerID == playerId)
            CardShowUI.GetComponent<CardShowUIScript>().cards = serializer.CurrentPlayer.DrawPile.ToArray();
        else
            CardShowUI.GetComponent<CardShowUIScript>().cards = serializer.EnemyPlayer.DrawPile.ToArray();
        CardShowUI.GetComponent<CardShowUIScript>().title.SetText("Draw Pile");
        CardShowUI.SetActive(true);
    }
}
