using System.Collections;
using System.Collections.Generic;
using TalesOfTribute;
using UnityEngine;
using TMPro;
using TalesOfTribute.Serializers;
using System.Linq;

public class AgentScript : MonoBehaviour
{
    private SerializedAgent _agent;
    public TextMeshPro Cost;
    public TextMeshPro Name;
    public TextMeshPro Type;
    public TextMeshPro Effects;
    public TextMeshPro HP;
    public SpriteRenderer activate;
    public Sprite[] CardSprites;

    private PlayerEnum _owner;

    public void SetUpCardInfo(SerializedAgent card, PlayerEnum owner)
    {
        _agent = card;
        _owner = owner;
        GetComponent<SpriteRenderer>().sprite = CardSprites.First(sprite => sprite.name == CardScript.ParseDeckAndType(card.RepresentingCard));
        Cost.SetText(card.RepresentingCard.Cost.ToString());
        Name.SetText(card.RepresentingCard.Name);
        Type.SetText(card.RepresentingCard.Type.ToString());
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
    
    public PlayerEnum GetOwner()
    {
        return _owner;
    }

    private void OnMouseEnter()
    {
        transform.localScale *= 1.5f;
    }

    private void OnMouseExit()
    {
        transform.localScale /= 1.5f;
    }

}
