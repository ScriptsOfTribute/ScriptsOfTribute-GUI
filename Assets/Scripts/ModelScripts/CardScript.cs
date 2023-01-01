using System.Collections;
using System.Collections.Generic;
using TalesOfTribute;
using UnityEngine;
using TMPro;
using System.Diagnostics.Contracts;
using System.Linq;

public class CardScript : MonoBehaviour
{
    private Card _card;
    public TextMeshPro Cost;
    public TextMeshPro Name;
    public TextMeshPro Type;
    public TextMeshPro Effects;
    private float _slot;
    public Sprite[] CardSprites;
    public void SetUpCardInfo(Card card)
    {
        _card = card;
        GetComponent<SpriteRenderer>().sprite = CardSprites.First(sprite => sprite.name == ParseDeckAndType(card));
        Cost.SetText(card.Cost.ToString());
        Name.SetText(card.Name);
        Type.SetText(TypeToString(card.Type));

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
        if (effects.Length > 50)
        {
            Effects.fontSize = 1f;
        }
        if (effects.Length > 110)
        {
            Effects.fontSize = 0.9f;
        }
        if (effects.Length > 130)
        {
            Effects.fontSize = 0.8f;
        }
        if (effects.Length > 150)
        {
            Effects.fontSize = 0.75f;
        }
    }

    public Card GetCard()
    {
        return _card;
    }

    private string TypeToString(CardType type)
    {
        return type switch
        {
            CardType.AGENT => "Agent",
            CardType.CURSE => "Curse",
            CardType.CONTRACT_AGENT => "Contract Agent",
            CardType.CONTRACT_ACTION => "Contract Action",
            CardType.STARTER => "Starter",
            CardType.ACTION => "Action",
            _ => ""
        };
    }

    private void OnMouseEnter()
    {
        if (GameManager.isUIActive)
            return;
        transform.localScale *= 1.5f;
        _slot = transform.position.z;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    private void OnMouseExit()
    {
        if (GameManager.isUIActive)
            return;
        transform.localScale /= 1.5f;
        transform.position = new Vector3(transform.position.x, transform.position.y, _slot);
    }

    public static string ParseDeckAndType(Card card)
    {
        string type = card.Type == CardType.AGENT || card.Type == CardType.CONTRACT_AGENT ? "Agent" : "Card";
        string deck = card.Deck switch
        {
            PatronId.ANSEI => "Ansei",
            PatronId.DUKE_OF_CROWS => "DukeOfCrows",
            PatronId.RAJHIN => "Rajhin",
            PatronId.ORGNUM => "Orgnum",
            PatronId.HLAALU => "Hlaalu",
            PatronId.PSIJIC => "Psijic",
            PatronId.PELIN => "Pelin",
            PatronId.RED_EAGLE => "RedEagle",
            PatronId.TREASURY => "Treasury",
            _ => ""
        };
        return $"{deck} {type}";
    }
}
