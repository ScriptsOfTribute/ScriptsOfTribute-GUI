using System;
using System.Collections.Generic;
using System.Text;
using TalesOfTribute;
using TalesOfTribute.Board;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EndGameUI : MonoBehaviour
{
    public GameObject Winner;
    public GameObject Reason;
    public GameObject AdditionalContext;
    public GameObject GameSeed;
    public GameObject Container;
    public GameObject MoveObject;
    public TMP_FontAsset FontAsset;
    private int _roundCounter = 1;
    private float _startY = -40f;
    private float _offset = 60f;
    private float _width = 400;
    private float _height = 50;

    public void SetUp(EndGameState state)
    {
        Winner.GetComponent<TextMeshProUGUI>().SetText(ParseWinner(state.Winner));
        Reason.GetComponent<TextMeshProUGUI>().SetText(ParseReason(state));
        //AdditionalContext.GetComponent<TextMeshProUGUI>().SetText(state.AdditionalContext);
        ShowMoves();
    }

    string ParseWinner(PlayerEnum winner)
    {
        if (PlayerScript.Instance.playerID == winner)
            return "Player won!";
        else
            return $"{TalesOfTributeAI.Instance.Name} won!";
    }

    string ParseReason(EndGameState state)
    {
        var loser = state.Winner == PlayerScript.Instance.playerID ? TalesOfTributeAI.Instance.Name : "Player";
        var winner = state.Winner == PlayerScript.Instance.playerID ? "Player" : TalesOfTributeAI.Instance.Name;
        return state.Reason switch
        {
            GameEndReason.INCORRECT_MOVE => $"{loser} made an incorrect move!",
            GameEndReason.MOVE_TIMEOUT => $"{loser} didn't make move in time!",
            GameEndReason.TURN_TIMEOUT => $"{loser} didn't finish turn in time!",
            GameEndReason.TURN_LIMIT_EXCEEDED => $"Turn limit exceeded!",
            GameEndReason.PRESTIGE_OVER_40_NOT_MATCHED => $"{loser} didn't match prestige over 40!",
            GameEndReason.PATRON_FAVOR => $"{winner} gained all patron favors!",
            GameEndReason.PATRON_SELECTION_FAILURE => $"{loser} failed selection of patron",
            GameEndReason.PATRON_SELECTION_TIMEOUT => $"{loser} didn't select patron in time!",
            GameEndReason.PRESTIGE_OVER_80 => $"{winner} reached 80 prestige before {loser}!",
            _ => throw new System.NotImplementedException(),
        };
    }

    private void ShowMoves()
    {
        List<CompletedAction> movesList = Logger.Instance.GetMoves();
        float currentOffset = 0f;
        string botName = TalesOfTributeAI.Instance.Name;
        string whoMoves = PlayerEnum.PLAYER1 == PlayerScript.Instance.playerID ? "Player" : botName;
        var roundObject = new GameObject($"Round nr. {_roundCounter}");
        roundObject.transform.SetParent(Container.transform);
        roundObject.AddComponent<TextMeshProUGUI>();
        roundObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, _height);
        roundObject.GetComponent<TextMeshProUGUI>().SetText($"Round nr. {_roundCounter}, moves: {whoMoves}");
        roundObject.GetComponent<TextMeshProUGUI>().fontSize = 28f;
        roundObject.GetComponent<TextMeshProUGUI>().font = FontAsset;
        roundObject.GetComponent<TextMeshProUGUI>().fontStyle = TMPro.FontStyles.Bold;
        foreach (CompletedAction move in movesList)
        {
            Debug.Log(move);
            var stringMove = ParseCompletedAction(move);
            var moveObject = Instantiate(MoveObject, Container.transform);
            moveObject.AddComponent<TextMeshProUGUI>();
            moveObject.GetComponent<TextMeshProUGUI>().SetText(stringMove);
            moveObject.GetComponent<TextMeshProUGUI>().fontSize = 20f;
            moveObject.GetComponent<TextMeshProUGUI>().font = FontAsset;
            moveObject.GetComponent<RectTransform>().sizeDelta = new Vector2((float)stringMove.Length * 15f, _height);
            moveObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, _startY - currentOffset);
            currentOffset += _offset;
            if (move.Type == CompletedActionType.END_TURN)
            {
                _roundCounter++;
                whoMoves = whoMoves == "Player" ? botName : "Player";
                roundObject = new GameObject($"Round nr. {_roundCounter}");
                roundObject.transform.SetParent(Container.transform);
                roundObject.AddComponent<RectTransform>();
                roundObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, _height);
                roundObject.AddComponent<TextMeshProUGUI>();
                roundObject.GetComponent<TextMeshProUGUI>().SetText($"Round nr. {_roundCounter}, moves: {whoMoves}");
                roundObject.GetComponent<TextMeshProUGUI>().fontSize = 28f;
                roundObject.GetComponent<TextMeshProUGUI>().font = FontAsset;
                roundObject.GetComponent<TextMeshProUGUI>().fontStyle = TMPro.FontStyles.Bold;
            }
        }
    }

    private string ParseCompletedAction(CompletedAction action)
    {
        var sb = new StringBuilder();

        var source = action.SourceCard != null ? action.SourceCard.Name : action.SourcePatron.ToString();

        switch (action.Type)
        {
            case CompletedActionType.BUY_CARD:
                sb.Append($"Buy Card - {action.TargetCard.Name}");
                break;
            case CompletedActionType.ACQUIRE_CARD:
                sb.Append($"Acquire Card - Source: {source}, Target: {action.TargetCard.Name}");
                break;
            case CompletedActionType.PLAY_CARD:
                sb.Append($"Play Card - {action.TargetCard.Name}");
                break;
            case CompletedActionType.ACTIVATE_AGENT:
                sb.Append($"Activate Agent - {action.TargetCard.Name}");
                break;
            case CompletedActionType.ACTIVATE_PATRON:
                sb.Append($"Activate Patron - {action.SourcePatron}");
                break;
            case CompletedActionType.ATTACK_AGENT:
                sb.Append($"Attack Agent - {action.TargetCard.Name} for {action.Amount}");
                break;
            case CompletedActionType.AGENT_DEATH:
                sb.Append($"Agent Death - {action.TargetCard.Name}");
                break;
            case CompletedActionType.GAIN_COIN:
                sb.Append($"Gain Coin - Amount: {action.Amount}, Source: {source}");
                break;
            case CompletedActionType.GAIN_POWER:
                sb.Append($"Gain Power - Amount: {action.Amount}, Source: {source}");
                break;
            case CompletedActionType.GAIN_PRESTIGE:
                sb.Append($"Gain Prestige - Amount: {action.Amount}, Source: {source}");
                break;
            case CompletedActionType.OPP_LOSE_PRESTIGE:
                sb.Append($"Opp Lose Prestige - Amount: {action.Amount}, Source: {source}");
                break;
            case CompletedActionType.REPLACE_TAVERN:
                sb.Append($"Replace Tavern - Source: {source}, Target: {action.TargetCard.Name}");
                break;
            case CompletedActionType.DESTROY_CARD:
                sb.Append($"Destroy Card - Source: {source}, Target: {action.TargetCard.Name}");
                break;
            case CompletedActionType.DRAW:
                sb.Append($"Draw - Amount: {action.Amount}, Source: {source}");
                break;
            case CompletedActionType.DISCARD:
                sb.Append($"Discard - Source: {source}, Target: {action.TargetCard.Name}");
                break;
            case CompletedActionType.REFRESH:
                sb.Append($"Refresh - Source: {source}, Target: {action.TargetCard.Name}");
                break;
            case CompletedActionType.TOSS:
                sb.Append($"Toss - Source: {source}, Target: {action.TargetCard.Name}");
                break;
            case CompletedActionType.KNOCKOUT:
                sb.Append($"Knockout - Source: {source}, Target: {action.TargetCard.Name}");
                break;
            case CompletedActionType.ADD_BOARDING_PARTY:
                sb.Append($"Add Boarding Party - Source: {source}");
                break;
            case CompletedActionType.ADD_BEWILDERMENT_TO_OPPONENT:
                sb.Append($"Add Bewilderment To Opponent - Source: {source}");
                break;
            case CompletedActionType.HEAL_AGENT:
                sb.Append($"Heal Agent: Amount: {action.Amount}, Agent: {action.TargetCard.Name}, Source: {source}");
                break;
            case CompletedActionType.END_TURN:
                sb.Append($"End Turn\n---------------");
                break;
            case CompletedActionType.ADD_PATRON_CALLS:
                sb.Append($"Increment patron calls - Amount: {action.Amount}, Source: {source}");
                break;
            case CompletedActionType.ADD_WRIT_OF_COIN:
                sb.Append($"Add Writ Of Coin - Source {source}");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return sb.ToString();
    }
}
