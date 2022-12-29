using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class MovesHistoryUI : MonoBehaviour
{
    public GameObject Container;
    private float _startY = -40f;
    private float _offset = 60f;
    private float _width = 400;
    private float _height = 50;
    private void OnEnable()
    {
        List<string> movesList = TalesOfTributeAI.Instance.GetMoves();
        float currentOffset = 0f;
        foreach(string move in movesList)
        {
            var textObject = new GameObject(move);
            textObject.AddComponent<TextMeshProUGUI>();
            textObject.GetComponent<TextMeshProUGUI>().SetText(move);
            textObject.GetComponent<TextMeshProUGUI>().fontSize = 20f;
            textObject.transform.SetParent(Container.transform, false);
            textObject.GetComponent<RectTransform>().sizeDelta = new Vector2(_width, _height);
            textObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, _startY - currentOffset);
            currentOffset += _offset;
        }
    }

    private void OnDisable()
    {
        for(int i = 0; i < Container.transform.childCount; i++)
        {
            Destroy(Container.transform.GetChild(i).gameObject);
        }
    }
}
