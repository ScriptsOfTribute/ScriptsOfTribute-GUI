using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TalesOfTribute;
using TMPro;

public class EffectButtonUIScript : MonoBehaviour
{
    private EffectType _effect;
    public GameObject description;

    public void OnClick()
    {
        Debug.Log(_effect);
        FindObjectOfType<EffectUIScript>().MakeChoice(_effect);
    }
    public void SetUpEffectInfo(EffectType effect)
    {
        _effect = effect;

        string desc = effect.ToString();
        description.GetComponent<TextMeshProUGUI>().SetText(desc);

    }
}
