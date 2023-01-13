using System;
using System.Reflection;
using TalesOfTribute.AI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PickBotButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Assembly Assembly;
    public Type Type;
    public TextMeshProUGUI text;
    public TextMeshProUGUI Description;

    public void Start()
    {
        GetComponent<Image>().color = Color.white;
    }

    public void OnClick()
    {
        TalesOfTributeAI.Instance.SetBotInstance(Assembly.CreateInstance(Type.FullName) as AI);
        TalesOfTributeAI.Instance.Name = Type.Name;
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            var button = transform.parent.GetChild(i).gameObject;
            button.GetComponent<Image>().color = Color.white;
        }
        GetComponent<Image>().color = Color.green;
        MainMenuScript.BotSelected = true;
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Description.SetText($"Module:\n{Assembly.FullName.Split(',')[0]}\n\n Namespace and Class name: {Type.FullName}");
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        Description.SetText("");
    }
}
