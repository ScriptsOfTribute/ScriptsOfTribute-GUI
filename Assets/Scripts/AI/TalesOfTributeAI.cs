using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TalesOfTribute;
using TalesOfTribute.AI;
using UnityEngine;

public class TalesOfTributeAI : MonoBehaviour
{
    public static TalesOfTributeAI Instance { get; private set; }
    public PlayerEnum botID { get; private set; }
    private AI bot { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        botID = PlayerEnum.PLAYER2;
    }

    public void SetBotInstance(AI botInstance)
    {
        bot = botInstance;
    }

    public void SetSide(PlayerEnum id)
    {
        this.botID = id;
    }

    public PatronId SelectPatron(List<PatronId> availablePatrons, int round)
    {
        return bot.SelectPatron(availablePatrons, round);
    }

    public Move Play(SerializedBoard serializedBoard, List<Move> possibleMoves)
    {
        var move =  bot.Play(serializedBoard, possibleMoves);
        MoveLogger.Instance.AddSimpleMove(move);
        return move;
    }

}
