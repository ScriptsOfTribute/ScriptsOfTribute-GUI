using System;
using System.Reflection;
using TalesOfTribute.AI;

using UnityEngine;
using UnityEngine.UI;

public class PickBotButtonScript : MonoBehaviour
{
    public Assembly Assembly;
    public Type Type;

    public void OnClick()
    {
        TalesOfTributeAI.Instance.SetBotInstance(Assembly.CreateInstance(Type.FullName) as AI);
        GetComponent<Image>().color = Color.magenta;
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).gameObject.GetComponent<Button>().interactable = false;
        }
        MainMenuScript.BotSelected = true;
    }
}
