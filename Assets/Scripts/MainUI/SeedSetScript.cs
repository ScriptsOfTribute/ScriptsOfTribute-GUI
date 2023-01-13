using TMPro;
using UnityEngine;

public class SeedSetScript : MonoBehaviour
{
    public TMP_InputField inputField;

    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onEndEdit.AddListener(SetTimeout);
    }

    void SetTimeout(string value)
    {
        ulong number;
        if (ulong.TryParse(value, out number))
        {
            BoardManager.Instance.SetSeed(number);
        }
        else
        {
            //Default value is set, 1000ms
        }
    }
}
