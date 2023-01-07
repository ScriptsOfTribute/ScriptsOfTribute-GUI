using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UITooltipHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Tooltip;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        Tooltip.SetActive(false);
    }
}
