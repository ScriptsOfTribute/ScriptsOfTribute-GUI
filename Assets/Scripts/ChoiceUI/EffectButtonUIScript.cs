using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TalesOfTribute;
using TMPro;

public class EffectButtonUIScript : MonoBehaviour
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

    }
}
