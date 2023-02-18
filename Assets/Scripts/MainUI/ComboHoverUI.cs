using System.Collections.Generic;
using ScriptsOfTribute;
using ScriptsOfTribute.Board.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ComboHoverUI : MonoBehaviour
{
    public GameObject Panel;
    public TMP_FontAsset Font;
    public GameObject TextObjectTemplate;

    public void SetUp(List<string> effects)
    {
        if(effects.Count == 0)
        {
            return;
        }
        foreach(var effect in effects)
        {
            var Text = Instantiate(TextObjectTemplate, Panel.transform);
            Text.GetComponent<TextMeshProUGUI>().SetText(effect);
            Text.GetComponent<TextMeshProUGUI>().font = Font;
            Text.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            Text.GetComponent<TextMeshProUGUI>().fontSize = 20;
            Text.SetActive(true);
        }
        Panel.SetActive(true);
    }

    public void Close()
    {
        for(int i = 1; i < Panel.transform.childCount; i++)
        {
            Destroy(Panel.transform.GetChild(i).gameObject);
        }
        Panel.SetActive(false);
    }
}
