using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoomer : MonoBehaviour
{
    public float ZoomValue = 1.5f;
    private float _previousZ;
    private float _previousY;

    private void OnMouseEnter()
    {
        if (GameManager.isUIActive)
            return;
        transform.localScale *= ZoomValue;
        _previousZ = transform.position.z;
        _previousY = transform.position.y;
        
        var leftBottom = Camera.main.ViewportToWorldPoint(Vector3.zero);
        var rightTop = Camera.main.ViewportToWorldPoint(Vector3.one);
        var sprite = GetComponent<SpriteRenderer>().sprite;
        var newY = transform.position.y;
        if (leftBottom.y > transform.position.y - sprite.bounds.size.y * transform.localScale.y/2)
        {
            newY += leftBottom.y - transform.position.y + sprite.bounds.size.y * transform.localScale.y / 2;
        }

        if (rightTop.y < transform.position.y + sprite.bounds.size.y * transform.localScale.y / 2)
        {
            newY += rightTop.y - transform.position.y - sprite.bounds.size.y * transform.localScale.y / 2;
        }
        transform.position = new Vector3(transform.position.x, newY, -3);
    }

    private void OnMouseExit()
    {
        if (GameManager.isUIActive)
            return;
        transform.localScale /= ZoomValue;
        transform.position = new Vector3(transform.position.x, _previousY, _previousZ);
    }
}
