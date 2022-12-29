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
    private List<string> _moves = new List<string>();

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
        if (move.Command == CommandEnum.MAKE_CHOICE)
        {
            if (move is MakeChoiceMove<Card> cardChoice)
            {
                _moves.Add($"\tCHOICE: {string.Join(',', cardChoice.Choices.Select(c => c.Name))}");
            }
            else if (move is MakeChoiceMove<Effect> effectChoice)
            {
                _moves.Add($"\tCHOICE: {string.Join(',', effectChoice.Choices.Select(e => e.Type))}");
            }
        }
        else
        {
            _moves.Add(move.ToString());
        }
        if (move.Command == CommandEnum.END_TURN)
        {
            _moves.Add("--------------");
        }
        return move;
    }

    public List<string> GetMoves()
    {
        return _moves;
    }
}
