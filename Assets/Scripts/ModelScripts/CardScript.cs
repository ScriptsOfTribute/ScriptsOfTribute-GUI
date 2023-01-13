using System.Collections.Generic;
using TalesOfTribute;
using UnityEngine;
using TMPro;
using System.Linq;
using TalesOfTribute.Board.Cards;
using TalesOfTribute.Serializers;

public class CardScript : MonoBehaviour
{
    private UniqueCard _card;
    public TextMeshPro Cost;
    public TextMeshPro Name;
    public TextMeshPro Type;
    public TextMeshPro Effects;
    public TextMeshPro HP;
    public Sprite[] CardSprites;
    private bool _tavernCardPlayerCanAfford;
    private List<string> _effectsWillEnact = new List<string>();
    public void SetUpCardInfo(UniqueCard card, ComboState comboState, bool _canAfford = true)
    {
        _card = card;
        GetComponent<SpriteRenderer>().sprite = CardSprites.First(sprite => sprite.name == ParseDeckAndType(card));
        Cost.SetText(card.Cost.ToString());
        Name.SetText(card.Name);
        Type.SetText(TypeToString(card.Type));
        if(card.HP > 0)
        {
            HP.SetText(card.HP.ToString());
        }
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

        _tavernCardPlayerCanAfford = _canAfford;
        if (!_canAfford)
        {
            var color = GetComponent<SpriteRenderer>().color;
            color.a = 200 / 255f;
            GetComponent<SpriteRenderer>().color = color;
        }
        if(comboState.CurrentCombo > 0 && card.Deck != PatronId.TREASURY)
        {
            if (card.Effects[comboState.CurrentCombo] != null)
            {
                foreach(var effect in card.Effects[comboState.CurrentCombo].Decompose())
                {
                    _effectsWillEnact.Add($"{effect} (this card)");
                }
            }
            _effectsWillEnact.AddRange(comboState.All[comboState.CurrentCombo].Select(e => e.ToString()).ToList());
        }
    }

    public UniqueCard GetCard()
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

    public bool CanPlayerAfford()
    {
        return _tavernCardPlayerCanAfford;
    }

    public static string ParseDeckAndType(UniqueCard card)
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

    private void OnMouseEnter()
    {
        if(_effectsWillEnact == null)
        {
            return;
        }

        if ((gameObject.tag == "Card" || _card.Type == CardType.CONTRACT_ACTION || _card.Type == CardType.CONTRACT_AGENT) && _card.Deck != PatronId.TREASURY)
            FindObjectOfType<ComboHoverUI>().SetUp(_effectsWillEnact);
    }

    private void OnMouseExit()
    {
        if ((gameObject.tag == "Card" || _card.Type == CardType.CONTRACT_ACTION || _card.Type == CardType.CONTRACT_AGENT) && _card.Deck != PatronId.TREASURY)
            FindObjectOfType<ComboHoverUI>().Close();
    }
}
