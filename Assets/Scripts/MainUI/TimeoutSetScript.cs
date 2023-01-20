using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeoutSetScript : MonoBehaviour
{
    public TMP_InputField inputField;

    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onEndEdit.AddListener(SetTimeout);
    }

    void SetTimeout(string value)
    {
        int number;
        if (int.TryParse(value, out number))
        {
            TalesOfTributeAI.Instance.SetTimeout(number);
        }
        else
        {
            //Default value is set, 1000ms
        }
    }
}
