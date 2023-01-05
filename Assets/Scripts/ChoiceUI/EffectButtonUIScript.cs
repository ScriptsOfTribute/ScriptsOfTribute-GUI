using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TalesOfTribute;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TalesOfTribute.Board.Cards;

public class EffectButtonUIScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UniqueEffect _effect;
    public GameObject description;

    public void OnClick()
    {
        FindObjectOfType<EffectUIScript>().MakeChoice(_effect);
    }
    public void SetUpEffectInfo(UniqueEffect effect)
    {
        _effect = effect;

        string desc = effect.ToString();
        description.GetComponent<TextMeshProUGUI>().SetText(desc);
        if (desc.Length > 30)
        {
            description.GetComponent<TextMeshProUGUI>().fontSize = 14;
        }
        else if (desc.Length > 55)
        {
            description.GetComponent<TextMeshProUGUI>().fontSize = 12;
        }
        else if (desc.Length > 65)
        {
            description.GetComponent<TextMeshProUGUI>().fontSize = 10;
        }

    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        GetComponent<Image>().fillCenter = true;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        GetComponent<Image>().fillCenter = false;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
    }
}
