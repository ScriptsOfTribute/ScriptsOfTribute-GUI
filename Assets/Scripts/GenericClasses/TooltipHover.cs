using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipHover : MonoBehaviour
{
    public GameObject Tooltip;

    private void Start()
    {
        var leftBottom = Camera.main.ViewportToWorldPoint(Vector3.zero);
        var rightTop   = Camera.main.ViewportToWorldPoint(Vector3.one);
        var sprite = Tooltip.GetComponent<SpriteRenderer>().sprite;
        if (leftBottom.y > Tooltip.transform.position.y - sprite.bounds.size.y)
        {
            Tooltip.transform.position += new Vector3(0, sprite.bounds.size.y, 0);
        }

        if (rightTop.y < Tooltip.transform.position.y + sprite.bounds.size.y)
        {
            Tooltip.transform.position -= new Vector3(0, sprite.bounds.size.y, 0);
        }
    }

    private void OnMouseEnter()
    {
        Tooltip.SetActive(true);
    }

    private void OnMouseExit()
    {
        Tooltip.SetActive(false);
    }
}
