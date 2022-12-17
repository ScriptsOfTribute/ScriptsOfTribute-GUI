using System.Collections;
using System.Collections.Generic;
using TalesOfTribute;
using UnityEngine;
using TMPro;

public class CardScript : MonoBehaviour
{
    private Card _card;
    public TextMeshPro Cost;
    public TextMeshPro Name;
    public TextMeshPro Type;
    public SpriteRenderer Deck;
    public TextMeshPro Effects;

    public Sprite[] decks;

    public void SetUpCardInfo(Card card)
    {
        _card = card;

        Cost.SetText(card.Cost.ToString());
        Name.SetText(card.Name);
        Type.SetText(card.Type.ToString());
        Deck.sprite = decks[(int)card.Deck];

        string effects = "";

        for(int i = 0; i < card.Effects.Length; i++)
        {
            if (i == 0 && card.Effects[0] != null) //Activation
            {
                effects += $"{card.Effects[i].ToString()}\n";
            }
            else if (card.Effects[i] != null)
            {
                effects += $"Combo {i + 1}: {card.Effects[i].ToString()}\n";
            }
        }

        Effects.SetText(effects);
    }

    public Card GetCard()
    {
        return _card;
    }
}
