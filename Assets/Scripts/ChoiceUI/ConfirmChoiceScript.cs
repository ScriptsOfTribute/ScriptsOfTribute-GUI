using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmChoiceScript : MonoBehaviour
{
    public void OnClick()
    {
        FindObjectOfType<CardChoiceUIScript>().MakeChoice();
    }
}
