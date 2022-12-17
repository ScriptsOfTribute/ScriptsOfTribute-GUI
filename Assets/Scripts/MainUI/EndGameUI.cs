using System.Collections;
using System.Collections.Generic;
using TalesOfTribute.Board;
using TMPro;
using UnityEngine;

public class EndGameUI : MonoBehaviour
{
    public GameObject Winner;
    public GameObject Reason;
    public GameObject AdditionalContext;

    public void SetUp(EndGameState state)
    {
        Winner.GetComponent<TextMeshProUGUI>().SetText(state.Winner.ToString());
        Reason.GetComponent<TextMeshProUGUI>().SetText(state.Reason.ToString());
        AdditionalContext.GetComponent<TextMeshProUGUI>().SetText(state.AdditionalContext);
    }
}
