using System.Collections;
using System.Collections.Generic;
using TalesOfTribute;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CardChoiceUIScript : MonoBehaviour
{
    public GameObject CardSlots;
    public GameObject cardPrefab;
    public GameObject ChoiceTopic;

    public Choice<Card> cardChoice;
    private List<Card> choicesSelected;

    private bool _completed;
    private int maxSelects;

    private void Start()
    {
        _completed = false;
    }
    public void SetUpChoices(Choice<Card> choice)
    {
        Debug.Log(choice.Context.ToString());
        ChoiceTopic.GetComponent<TextMeshProUGUI>().SetText(choice.Context.ChoiceType.ToString() + " " + choice.MaxChoiceAmount);
        _completed = false;
        choicesSelected = new List<Card>();
        cardChoice = choice;
        for(int i = 0; i < choice.PossibleChoices.Count; i++)
        {
            GameObject c = Instantiate(cardPrefab, CardSlots.transform.GetChild(i).transform);
            c.GetComponent<CardUIButtonScript>().SetUpCardInfo(choice.PossibleChoices[i]);
        }
    }

    void CleanUpChoices()
    {
        ChoiceTopic.GetComponent<TextMeshProUGUI>().SetText("");
        for (int i = 0; i < CardSlots.transform.childCount; i++)
        {
            if (CardSlots.transform.GetChild(i).transform.childCount > 0)
                Destroy(CardSlots.transform.GetChild(i).transform.GetChild(0).gameObject);
        }
    }

    public void SelectCard(Card c)
    {
        choicesSelected.Add(c);
    }

    public void UnSelectCard(Card c)
    {
        choicesSelected.Remove(c);
    }

    public void MakeChoice()
    {
        GameManager.Board.MakeChoice<Card>(choicesSelected);
        CleanUpChoices();
        _completed = true;
    }

    public bool GetCompletedStatus()
    {
        return _completed;
    }
}
