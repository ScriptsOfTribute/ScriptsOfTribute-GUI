using System.Collections;
using System.Collections.Generic;
using ScriptsOfTribute;
using UnityEngine;

public class CooldownPileButton : MonoBehaviour
{
    public GameObject CardShowUI;
    public bool isBot;

    public void OnClick()
    {
        var serializer = GameManager.Board.GetFullGameState();
        var playerId = isBot ? ScriptsOfTributeAI.Instance.botID : PlayerScript.Instance.playerID;
        if (serializer.CurrentPlayer.PlayerID == playerId)
            CardShowUI.GetComponent<CardShowUIScript>().cards = serializer.CurrentPlayer.CooldownPile.ToArray();
        else
            CardShowUI.GetComponent<CardShowUIScript>().cards = serializer.EnemyPlayer.CooldownPile.ToArray();
        CardShowUI.GetComponent<CardShowUIScript>().title.SetText("Cooldown Pile");
        CardShowUI.SetActive(true);
    }
}
