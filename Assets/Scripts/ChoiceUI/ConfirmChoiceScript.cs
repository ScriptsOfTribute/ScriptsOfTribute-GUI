using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmChoiceScript : MonoBehaviour
{
    public Button button;
    void Update()
    {
        button.interactable = FindObjectOfType<CardChoiceUIScript>().EnoughChoices();
    }
    public void OnClick()
    {
        GetComponent<Image>().fillCenter = true;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
        FindObjectOfType<CardChoiceUIScript>().MakeChoice();
    }
}
