using System.Collections;
using System.Collections.Generic;
using TalesOfTribute;
using UnityEngine;
using TMPro;
using TalesOfTribute.Serializers;

public class AgentScript : MonoBehaviour
{
    private SerializedAgent _agent;
    public TextMeshPro Cost;
    public TextMeshPro Name;
    public TextMeshPro Type;
    public SpriteRenderer Deck;
    public TextMeshPro Effects;
    public TextMeshPro HP;
    public SpriteRenderer activate;

    public Sprite[] decks;

    public void SetUpCardInfo(SerializedAgent card)
    {
        _agent = card;

        Cost.SetText(card.RepresentingCard.Cost.ToString());
        Name.SetText(card.RepresentingCard.Name);
        Type.SetText(card.RepresentingCard.Type.ToString());
        Deck.sprite = decks[(int)card.RepresentingCard.Deck];
        HP.SetText(card.CurrentHp.ToString());

        string effects = "";

        for(int i = 0; i < card.RepresentingCard.Effects.Length; i++)
        {
            if (i == 0 && card.RepresentingCard.Effects[0] != null) //Activation
            {
                effects += $"{card.RepresentingCard.Effects[i]}\n";
            }
            else if (card.RepresentingCard.Effects[i] != null)
            {
                effects += $"Combo {i + 1}: {card.RepresentingCard.Effects[i]}\n";
            }
        }

        Effects.SetText(effects);
        activate.gameObject.SetActive(card.Activated);
    }

    public SerializedAgent GetAgent()
    {
        return _agent;
    }

}
