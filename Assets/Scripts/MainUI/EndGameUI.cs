using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TalesOfTribute;
using TalesOfTribute.Board;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class EndGameUI : MonoBehaviour
{
    public GameObject Winner;
    public GameObject Reason;
    public GameObject AdditionalContext;
    public GameObject GameSeed;
    public GameObject Container;
    public GameObject MoveObject;
    public TMP_FontAsset FontAsset;
    public GameObject BlackFade;
    private int _roundCounter = 1;
    private float _startY = -40f;
    private float _offset = 60f;
    private float _width = 400;
    private float _height = 50;

    public IEnumerator SetUp(EndGameState state)
    {
        Winner.GetComponent<TextMeshProUGUI>().SetText(ParseWinner(state.Winner));
        Reason.GetComponent<TextMeshProUGUI>().SetText(ParseReason(state));
        //AdditionalContext.GetComponent<TextMeshProUGUI>().SetText(state.AdditionalContext);
        ShowMoves();
        GameSeed.GetComponent<TMP_InputField>().text = BoardManager.Instance.GetSeed().ToString();
        float fadeAmount = 5f;
        Color objectColor = BlackFade.GetComponent<UnityEngine.UI.Image>().color;
        while (BlackFade.GetComponent<UnityEngine.UI.Image>().color.a > 0)
        {
            fadeAmount = objectColor.a - (3f * Time.deltaTime);
            objectColor = new Color(0, 0, 0, fadeAmount);
            BlackFade.GetComponent<UnityEngine.UI.Image>().color = objectColor;
            yield return null;
        }
        yield return null;
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
        var textObject = new GameObject($"Round nr. {_roundCounter}");
        textObject.transform.SetParent(Container.transform);
        textObject.AddComponent<TextMeshProUGUI>();
        textObject.GetComponent<RectTransform>().sizeDelta = new Vector2(600, _height);
        textObject.GetComponent<TextMeshProUGUI>().SetText($"<size=110%><b>Round nr. {_roundCounter}, moves: {whoMoves}</b><size=100%>\n");
        textObject.GetComponent<TextMeshProUGUI>().fontSize = 28f;
        textObject.GetComponent<TextMeshProUGUI>().font = FontAsset;
        textObject.GetComponent<TextMeshProUGUI>().lineSpacing = 5;
        foreach (CompletedAction move in movesList)
        {
            var stringMove = ParseCompletedAction(move);
            textObject.GetComponent<TextMeshProUGUI>().text += $"{stringMove}\n";
            currentOffset += _offset;
            if (move.Type == CompletedActionType.END_TURN)
            {
                _roundCounter++;
                whoMoves = whoMoves == "Player" ? botName : "Player";
                textObject.GetComponent<TextMeshProUGUI>().text += $"<size=110%><b>Round nr. {_roundCounter}, moves: {whoMoves}\n</b><size=100%>";
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
