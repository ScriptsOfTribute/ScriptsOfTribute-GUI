using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject TextObject;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TextObject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        TextObject.SetActive(false);
    }
}
