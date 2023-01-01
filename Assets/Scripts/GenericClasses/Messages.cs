using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Messages : MonoBehaviour
{
    public static IEnumerator ShowMessage(GameObject TextObject, string message, float seconds)
    {
        TextObject.GetComponent<TextMeshProUGUI>().SetText(message);
        yield return new WaitForSeconds(seconds);
        TextObject.GetComponent<TextMeshProUGUI>().SetText("");
    }
}
