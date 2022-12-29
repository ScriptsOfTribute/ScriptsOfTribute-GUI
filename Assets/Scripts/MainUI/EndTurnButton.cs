using System.Collections;
using System.Collections.Generic;
using TalesOfTribute;
using UnityEngine;

public class EndTurnButton : MonoBehaviour
{
    public void OnClickEndTurn()
    {
        if (!GameManager.isBotPlaying)
            FindObjectOfType<GameManager>().GetComponent<GameManager>().EndTurn();
    }
}
