using System.Collections;
using System.Collections.Generic;
using TalesOfTribute;
using UnityEngine;
using TMPro;

public class CardUIButtonScript : MonoBehaviour
{
    private Card _card;
    public GameObject description;
    public GameObject effects;
    public GameObject checkmark;

    public void OnClick()
    {
        checkmark.SetActive(!checkmark.activeSelf);
        if (checkmark.activeSelf)
            FindObjectOfType<CardChoiceUIScript>().SelectCard(_card);
        else
            FindObjectOfType<CardChoiceUIScript>().UnSelectCard(_card);
    }

    public void SetUpCardInfo(Card card)
    {
        _card = card;

        string desc = $"{card.Name}\nCost: {card.Cost}, Type: {card.Type}\nDeck:{card.Deck}";

        description.GetComponent<TextMeshProUGUI>().SetText(desc);

        string eff = "";
        for (int i = 0; i < card.Effects.Length; i++)
        {
            if (card.Effects[i] != null)
                eff += $"{i}. {card.Effects[i].ToString()}\n";
        }

        effects.GetComponent<TextMeshProUGUI>().SetText(eff);
    }

    public Card GetCard()
    {
        return _card;
    }
}
