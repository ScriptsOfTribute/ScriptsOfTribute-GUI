using TMPro;
using UnityEngine;

public class BotLogsScript : MonoBehaviour
{
    public static BotLogsScript Instance;
    public GameObject ScrollView;
    public GameObject LogsPanel;
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

    private void CreateTextObject(string text)
    {
        var textObject = new GameObject();
        textObject.transform.SetParent(LogsPanel.transform);
        //
        textObject.AddComponent<TextMeshProUGUI>();
        textObject.GetComponent<TextMeshProUGUI>().SetText(text);
        textObject.GetComponent<TextMeshProUGUI>().font = Font;
        textObject.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
        textObject.GetComponent<TextMeshProUGUI>().enableAutoSizing = true;
        textObject.GetComponent<TextMeshProUGUI>().fontSizeMin = 14f;
        textObject.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 50f);
        textObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
    }

    void OnPanelEnable()
    {
        var logs = Logger.Instance.Logs;
        foreach(string log in logs)
        {
            CreateTextObject(log);
        }
    }

    void OnPanelDisable()
    {
        for(int i = 0; i < LogsPanel.transform.childCount; i++)
        {
            Destroy(LogsPanel.transform.GetChild(i).gameObject);
        }
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


