using System.Collections;
using System.Collections.Generic;
using TalesOfTribute;
using UnityEngine;

public class CooldownPileButton : MonoBehaviour
{
    public GameObject CardShowUI;
    public bool isBot;

    public void OnClick()
    {
        var serializer = GameManager.Board.GetSerializer();
        var playerId = isBot ? TalesOfTributeAI.Instance.botID : PlayerScript.Instance.playerID;
        if (serializer.CurrentPlayer.PlayerID == playerId)
            CardShowUI.GetComponent<CardShowUIScript>().cards = serializer.CurrentPlayer.CooldownPile.ToArray();
        else
            CardShowUI.GetComponent<CardShowUIScript>().cards = serializer.EnemyPlayer.CooldownPile.ToArray();
        GameManager.isUIActive = true;
        CardShowUI.SetActive(true);
    }
}
