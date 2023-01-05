using System;
using System.Reflection;
using TalesOfTribute.AI;

using UnityEngine;
using UnityEngine.UI;

public class PickBotButtonScript : MonoBehaviour
{
    public Assembly Assembly;
    public Type Type;

    public void Start()
    {
        GetComponent<Image>().color = Color.white;
    }

    public void OnClick()
    {
        TalesOfTributeAI.Instance.SetBotInstance(Assembly.CreateInstance(Type.FullName) as AI);
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            var button = transform.parent.GetChild(i).gameObject;
            button.GetComponent<Image>().color = Color.white;
        }
        GetComponent<Image>().color = Color.magenta;
        MainMenuScript.BotSelected = true;
    }
}
