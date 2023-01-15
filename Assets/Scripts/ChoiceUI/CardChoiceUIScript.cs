using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TalesOfTribute;
using TalesOfTribute.Serializers;
using TMPro;
using TalesOfTribute.Board.Cards;
using UnityEngine;
using System.Linq;

public class CardChoiceUIScript : MonoBehaviour
{
    public GameObject Container;
    public GameObject cardPrefab;
    public GameObject ChoiceTopic;
    public GameObject Panel;
    public TextMeshProUGUI TooltipText;

    public SerializedChoice cardChoice;
    private List<GameObject> choicesSelected;

    private bool _completed;

    private void Start()
    {
        _completed = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Panel.SetActive(!Panel.activeSelf);
            if (Panel.activeSelf)
            {
                SetUpChoices(cardChoice);
                TooltipText.SetText("[Space] Minimize");
            }
            else
            {
                CleanUpChoices();
                TooltipText.SetText("[Space] Show");
            }
        }
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
            text += $"{ParseChoiceFollowUp(choice.ChoiceFollowUp)}";
        }
        ChoiceTopic.GetComponent<TextMeshProUGUI>().SetText(text + $"- Min. {choice.MinChoices}, Max. {choice.MaxChoices}");
        _completed = false;
        choicesSelected = new List<GameObject>();
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

    public bool SelectCard(GameObject cardObject)
    {
        if (choicesSelected.Count > cardChoice.MaxChoices)
            return false;
        if (choicesSelected.Count == cardChoice.MaxChoices)
            UnSelectCard(choicesSelected[0]);
        choicesSelected.Add(cardObject);
        cardObject.GetComponent<CardUIButtonScript>().checkmark.SetActive(true);
        return true;
    }

    public void UnSelectCard(GameObject cardObject)
    {
        choicesSelected.Remove(cardObject);
        cardObject.GetComponent<CardUIButtonScript>().checkmark.SetActive(false);
    }

    public void MakeChoice()
    {
        GameManager.Board.MakeChoice(choicesSelected.Select(obj => obj.GetComponent<CardUIButtonScript>().GetCard()).ToList());
        CleanUpChoices();
        _completed = true;
    }

    public bool EnoughChoices()
    {
        return cardChoice.MinChoices <= choicesSelected.Count;
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
