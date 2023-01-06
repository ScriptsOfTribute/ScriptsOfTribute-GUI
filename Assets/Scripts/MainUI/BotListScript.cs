using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TalesOfTribute.AI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class BotListScript : MonoBehaviour
{
    public GameObject ButtonPrefab;
    public GameObject Container;
    public TMP_FontAsset font;

    void Start()
    {
        string[] dlls = Directory.GetFiles(Application.streamingAssetsPath, "*.dll");

        foreach(var dll in dlls)
        {
            var asm = Assembly.LoadFile(dll);
            foreach (var t in asm.GetTypes())
            {
                if (t.IsSubclassOf(typeof(AI)))
                {
                    var b = Instantiate(ButtonPrefab, Container.transform);
                    b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(t.Name);
                    b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().font = font;
                    b.GetComponent<PickBotButtonScript>().Assembly = asm;
                    b.GetComponent<PickBotButtonScript>().Type = t;
                }
            }
        }
    }

}
