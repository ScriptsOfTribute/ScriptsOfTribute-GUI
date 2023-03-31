using TMPro;
using UnityEngine;

public class BotLogsScript : MonoBehaviour
{
    public static BotLogsScript Instance;
    public GameObject ScrollView;
    public TMP_InputField InputField;
    public TMP_FontAsset Font;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void OnClick()
    {
        if (ScrollView.activeSelf)
        {
            OnPanelDisable();
            ScrollView.SetActive(false);
        }
        else
        {
            ScrollView.SetActive(true);
            OnPanelEnable();
        }
    }

    private void AddLog(string text)
    {
        InputField.text += text;
    }

    void OnPanelEnable()
    {
        var logs = Logger.Instance.Logs;
        foreach(string log in logs)
        {
            AddLog(log);
        }
    }

    public void OnPanelDisable()
    {
        InputField.text = "";
    }

    public void Clear()
    {
        Logger.Instance.Logs.Clear();
        Refresh();
    }

    public void Refresh()
    {
        if (ScrollView.activeSelf)
        {
            OnPanelDisable();
            OnPanelEnable();
        }
    }
}


