using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TalesOfTribute;
using TalesOfTribute.Serializers;
using TMPro;
using TalesOfTribute.Board.Cards;
using UnityEngine;

public class CardChoiceUIScript : MonoBehaviour
{
    public GameObject Container;
    public GameObject cardPrefab;
    public GameObject ChoiceTopic;

    public SerializedChoice cardChoice;
    private List<UniqueCard> choicesSelected;

    private bool _completed;
    private int maxSelects;

    private void Start()
    {
        _completed = false;
    }
    public void SetUpChoices(SerializedChoice choice)
    {
        string text = "";
        if (choice.Context.ChoiceType == ChoiceType.PATRON_ACTIVATION)
        {
            text += $"Patron {ParseChoiceFollowUp(choice.ChoiceFollowUp)}";
        }
        else if (choice.Context.ChoiceType == ChoiceType.CARD_EFFECT)
        {
            text += $"Card {ParseChoiceFollowUp(choice.ChoiceFollowUp)}";
        }
        ChoiceTopic.GetComponent<TextMeshProUGUI>().SetText(text + $"- Min. {choice.MinChoices}, Max. {choice.MaxChoices}");
        _completed = false;
        choicesSelected = new List<UniqueCard>();
        cardChoice = choice;
        for(int i = 0; i < choice.PossibleCards.Count; i++)
        {
            GameObject c = Instantiate(cardPrefab, Container.transform);
            c.GetComponent<CardUIButtonScript>().SetUpCardInfo(choice.PossibleCards[i]);
        }
    }

    void CleanUpChoices()
    {
        ChoiceTopic.GetComponent<TextMeshProUGUI>().SetText("");
        for (int i = 0; i < Container.transform.childCount; i++)
        {
            Destroy(Container.transform.GetChild(i).gameObject);
        }
    }

    public void SelectCard(UniqueCard c)
    {
        choicesSelected.Add(c);
    }

    public void UnSelectCard(UniqueCard c)
    {
        choicesSelected.Remove(c);
    }

    public void MakeChoice()
    {
        GameManager.Board.MakeChoice(choicesSelected);
        MoveLogger.Instance.AddSimpleMove(Move.MakeChoice(choicesSelected));
        CleanUpChoices();
        _completed = true;
    }

    public bool GetCompletedStatus()
    {
        return _completed;
    }

    string ParseChoiceFollowUp(ChoiceFollowUp choice)
    {
        return choice switch
        {
            ChoiceFollowUp.ENACT_CHOSEN_EFFECT => "",
            ChoiceFollowUp.REPLACE_CARDS_IN_TAVERN => "Replace cards in tavern",
            ChoiceFollowUp.DESTROY_CARDS => "Destroy cards",
            ChoiceFollowUp.DISCARD_CARDS => "Discard cards",
            ChoiceFollowUp.REFRESH_CARDS => "Refresh cards",
            ChoiceFollowUp.TOSS_CARDS => "Toss cards",
            ChoiceFollowUp.KNOCKOUT_AGENTS => "Knockout agents",
            ChoiceFollowUp.ACQUIRE_CARDS => "Acquire cards from tavern",
            ChoiceFollowUp.COMPLETE_HLAALU => "Hlaalu",
            ChoiceFollowUp.COMPLETE_PELLIN => "Pelin",
            ChoiceFollowUp.COMPLETE_PSIJIC => "Psijic",
            ChoiceFollowUp.COMPLETE_TREASURY => "Treasury",
            _ => "",
        };
    }
}
