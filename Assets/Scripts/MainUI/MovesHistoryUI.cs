using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ScriptsOfTribute;
using System.Linq;
using ScriptsOfTribute.Board;
using System.Text;
using System;
using ScriptsOfTribute.Board.Cards;

public class MovesHistoryUI : MonoBehaviour
{
    public GameObject Container;
    public GameObject CardHolder;
    public GameObject SourceCardHolder;
    public GameObject TargetCardHolder;
    public GameObject MoveObject;
    public TMP_FontAsset FontAsset;
    private float _startY = -40f;
    private float _offset = 60f;
    private float _width = 400;
    private float _height = 50;
    private bool lever = true;
    private int _roundCounter = 1;

    private void OnEnable()
    {
        lever = true;
        _roundCounter = 1;
        CleanView();
        ShowAdvancedMoves();
    }
    private void OnDisable()
    {
        CleanView();
    }

    void CleanView()
    {
        for (int i = 0; i < Container.transform.childCount; i++)
        {
            Destroy(Container.transform.GetChild(i).gameObject);
        }
    }

    public static string ParseMove(Move move)
    {
        if (move.Command == CommandEnum.MAKE_CHOICE)
        {
            if (move is MakeChoiceMove<UniqueCard> cardChoice)
            {
                return $"\tChoice: {string.Join(',', cardChoice.Choices.Select(c => c.Name))}";
            }
            else if (move is MakeChoiceMove<UniqueEffect> effectChoice)
            {
                return $"\tChoice: {string.Join(',', effectChoice.Choices.Select(e => e.Type))}";
            }
        }
        else if (move.Command == CommandEnum.END_TURN)
        {
            return "End turn\n---------------";
        }
        else if(move is SimpleCardMove cardMove)
        {
            return cardMove.Command switch
            {
                CommandEnum.PLAY_CARD => $"Play {cardMove.Card.Name}",
                CommandEnum.BUY_CARD => $"Buy {cardMove.Card.Name}",
                CommandEnum.ATTACK => $"Attack {cardMove.Card.Name}",
                CommandEnum.ACTIVATE_AGENT => $"Activate {cardMove.Card.Name}",
                _ => "",
            };
        }
        else if (move is SimplePatronMove patronMove)
        {
            return $"Patron {patronMove.PatronId}";
        }

        return move.ToString();
    }

    public void ShowAdvancedMoves()
    {
        List<CompletedAction> movesList = Logger.Instance.GetMoves();
        float currentOffset = 0f;
        string botName = ScriptsOfTributeAI.Instance.Name;
        string whoMoves = PlayerEnum.PLAYER1 == PlayerScript.Instance.playerID ? "Player" : botName;
        var roundObject = new GameObject($"Round nr. {_roundCounter}");
        roundObject.transform.SetParent(Container.transform);
        roundObject.AddComponent<TextMeshProUGUI>();
        roundObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, _height);
        roundObject.GetComponent<TextMeshProUGUI>().SetText($"Round nr. {_roundCounter}, moves: {whoMoves}");
        roundObject.GetComponent<TextMeshProUGUI>().fontSize = 28f;
        roundObject.GetComponent<TextMeshProUGUI>().font = FontAsset;
        roundObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
        foreach (CompletedAction move in movesList)
        {
            var stringMove = ParseCompletedAction(move);
            var moveObject = Instantiate(MoveObject, Container.transform);
            moveObject.GetComponent<CardMoveUI>().CardHolder = CardHolder;
            moveObject.GetComponent<CardMoveUI>().TargetCardHolder = TargetCardHolder;
            moveObject.GetComponent<CardMoveUI>().SourceCardHolder = SourceCardHolder;
            moveObject.GetComponent<TextMeshProUGUI>().SetText(stringMove);
            moveObject.GetComponent<TextMeshProUGUI>().fontSize = 20f;
            moveObject.GetComponent<TextMeshProUGUI>().font = FontAsset;
            moveObject.GetComponent<RectTransform>().sizeDelta = new Vector2((float)stringMove.Length * 15f, _height);
            moveObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, _startY - currentOffset);
            if (move.SourceCard != null && move.TargetCard != null)
            {
                moveObject.GetComponent<CardMoveUI>().TargetCard = move.TargetCard;
                moveObject.GetComponent<CardMoveUI>().SourceCard = move.SourceCard;
            }
            else if (move.SourceCard != null)
            {
                moveObject.GetComponent<CardMoveUI>().SingleCard = move.SourceCard;
            }
            else if (move.TargetCard != null)
            {
                moveObject.GetComponent<CardMoveUI>().SingleCard = move.TargetCard;
            }
            currentOffset += _offset;
            if(move.Type == CompletedActionType.END_TURN)
            {
                _roundCounter++;
                whoMoves = whoMoves == "Player" ? botName : "Player";
                roundObject = new GameObject($"Round nr. {_roundCounter}");
                roundObject.transform.SetParent(Container.transform);
                roundObject.AddComponent<TextMeshProUGUI>();
                roundObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, _height);
                roundObject.GetComponent<TextMeshProUGUI>().SetText($"Round nr. {_roundCounter}, moves: {whoMoves}");
                roundObject.GetComponent<TextMeshProUGUI>().fontSize = 28f;
                roundObject.GetComponent<TextMeshProUGUI>().font = FontAsset;
                roundObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
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
