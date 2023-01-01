using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TalesOfTribute;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EffectButtonUIScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Effect _effect;
    public GameObject description;

    public void OnClick()
    {
        FindObjectOfType<EffectUIScript>().MakeChoice(_effect);
    }
    public void SetUpEffectInfo(Effect effect)
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
