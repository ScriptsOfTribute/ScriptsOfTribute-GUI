using System.Collections;
using System.Collections.Generic;
using TalesOfTribute;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    public Button button;
    private void Update()
    {
        button.interactable = !GameManager.isBotPlaying;
    }
    public void OnClickEndTurn()
    {
        if (!GameManager.isBotPlaying)
        {
            FindObjectOfType<GameManager>().GetComponent<GameManager>().EndTurn();
        }
            
    }
}
