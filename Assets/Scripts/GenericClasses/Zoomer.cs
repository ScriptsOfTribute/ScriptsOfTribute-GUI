using UnityEngine;

public class Zoomer : MonoBehaviour
{
    public float ZoomValue = 1.5f;
    private float _previousZ;
    private float _previousY;
    private bool _rotated = false;
    private Quaternion _previousRotation;

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
            newY += rightTop.y - transform.position.y - sprite.bounds.size.y * transform.localScale.y / 2 + 0.5f;
        }
        transform.position = new Vector3(transform.position.x, newY, -3);

        _previousRotation = transform.rotation;
        var targetAngles = Vector3.zero;
        transform.eulerAngles = targetAngles;
    }

    private void OnMouseExit()
    {
        if (GameManager.isUIActive)
            return;
        transform.localScale /= ZoomValue;
        transform.position = new Vector3(transform.position.x, _previousY, _previousZ);
        transform.rotation = _previousRotation;
            
    }
}
