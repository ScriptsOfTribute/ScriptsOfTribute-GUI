using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TalesOfTribute;

public class CloseCardUI : MonoBehaviour
{
    public GameObject CardShowUI;

    public void OnClick()
    {
        CardShowUI.SetActive(false);
        GameManager.isUIActive = false;
    }
}
