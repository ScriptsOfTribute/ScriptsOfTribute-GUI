using System.Collections;
using System.Collections.Generic;
using TalesOfTribute;
using UnityEngine;

public class MovesHistoryButton : MonoBehaviour
{
    public GameObject MovesHistoryUI;

    public void OnClick()
    {
        GameManager.isUIActive = true;
        MovesHistoryUI.SetActive(true);
    }

    public void Close()
    {
        GameManager.isUIActive = false;
        MovesHistoryUI.SetActive(false);
    }
}
