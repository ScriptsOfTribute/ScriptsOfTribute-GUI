using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatronSelectionButton : MonoBehaviour
{
    public void OnClick()
    {
        FindObjectOfType<PatronSelectionScript>().PatronClicked(this.gameObject);
        this.GetComponent<Button>().enabled = false;
    }
}
