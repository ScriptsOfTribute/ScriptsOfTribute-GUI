using System.Collections;
using System.Collections.Generic;
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
        FindObjectOfType<CardChoiceUIScript>().MakeChoice();
    }
}
