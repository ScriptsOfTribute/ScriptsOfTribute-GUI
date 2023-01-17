using System.Collections;
using System.Collections.Generic;
using TalesOfTribute;
using UnityEngine;
using TMPro;
using TalesOfTribute.Serializers;
using System.Linq;
using TalesOfTribute.Board.Cards;

public class AgentScript : MonoBehaviour
{
    private SerializedAgent _agent;
    public TextMeshPro Cost;
    public TextMeshPro Name;
    public TextMeshPro Type;
    public TextMeshPro Effects;
    public TextMeshPro HP;
    public SpriteRenderer activate;
    public GameObject Taunt;
    public Sprite[] CardSprites;
    private List<string> _effectsWillEnact = new List<string>();

    private PlayerEnum _owner;

    public void SetUpCardInfo(SerializedAgent card, ComboState comboState, PlayerEnum owner)
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
        Taunt.SetActive(card.RepresentingCard.Taunt);

        if (comboState.CurrentCombo > 0 && card.RepresentingCard.Deck != PatronId.TREASURY)
        {
            for (int i = 1; i <= comboState.CurrentCombo; i++)
            {
                if (card.RepresentingCard.Effects[i] != null)
                {
                    foreach (var effect in card.RepresentingCard.Effects[i].Decompose())
                    {
                        _effectsWillEnact.Add($"{effect} (this card)");
                    }
                }
                _effectsWillEnact.AddRange(comboState.All[i].Select(e => e.ToString()).ToList());
            }

        }
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
        if (_effectsWillEnact == null || _agent.Activated)
        {
            return;
        }
        FindObjectOfType<ComboHoverUI>().SetUp(_effectsWillEnact);
    }

    private void OnMouseExit()
    {
        FindObjectOfType<ComboHoverUI>().Close();
    }
}
