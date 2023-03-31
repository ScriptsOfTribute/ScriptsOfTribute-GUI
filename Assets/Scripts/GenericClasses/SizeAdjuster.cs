using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeAdjuster : MonoBehaviour
{
    public GameObject objectToAdjust;
    public float desiredScale = 1.1f;
    void Start()
    {
        Vector3 parentScaleVector = this.transform.parent.transform.localScale;
        // parentScale * scaler = desiredScale
        var scalerX = desiredScale / parentScaleVector.x;
        var scalerY = desiredScale / parentScaleVector.y;
        Vector3 newScale = new Vector3(
            objectToAdjust.transform.localScale.x * scalerX,
            objectToAdjust.transform.localScale.y * scalerY,
            1
        );
        objectToAdjust.transform.localScale = newScale;
    }

}
