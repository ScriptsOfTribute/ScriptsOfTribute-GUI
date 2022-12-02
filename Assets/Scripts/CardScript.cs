using System.Collections;
using System.Collections.Generic;
using TalesOfTribute;
using UnityEngine;
using TMPro;

public class CardScript : MonoBehaviour
{
    private Card _card;
    public TextMeshPro description;
    public TextMeshPro effects;

    public void SetUpCardInfo(Card card)
    {
        _card = card;

        string desc = $"{card.Name}\nCost: {card.Cost}, Type: {card.Type}\nDeck:{card.Deck}";

        description.SetText(desc);

        string eff = "";
        for(int i = 0; i < card.Effects.Length; i++)
        {
            if (card.Effects[i] != null)
                eff += $"{i}. {card.Effects[i].ToString()}\n";
        }

        effects.SetText(eff);
    }

    public Card GetCard()
    {
        return _card;
    }
}
