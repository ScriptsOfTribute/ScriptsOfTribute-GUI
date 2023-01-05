using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class DropdownScript : MonoBehaviour
{
    public GameObject Panel;
    public void OnClick()
    {
        if (!Panel.activeSelf)
        {
            Panel.SetActive(true);
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else
        {
            Panel.SetActive(false);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        
    }
}
