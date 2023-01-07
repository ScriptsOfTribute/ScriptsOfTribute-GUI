using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollMouseWheel : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float scrollSensitivity = 100.0f;

    void Update()
    {
        //// Check if the mouse wheel was scrolled
        //float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
        //if (scrollAmount != 0.0f)
        //{
        //    // Calculate the scroll delta based on the scroll sensitivity
        //    float scrollDelta = scrollAmount + scrollSensitivity;
        //    Debug.Log(scrollDelta);
        //    Debug.Log(scrollAmount);

        //    // Create a scroll event and send it to the scroll rect
        //    ScrollEvent scrollEvent = new ScrollEvent(null, scrollDelta);
        //    ExecuteEvents.Execute(scrollRect.gameObject, scrollEvent, ExecuteEvents.scrollHandler);
        //}
    }
}

// Define the ScrollEvent class
public class ScrollEvent : BaseEventData
{
    public float scrollDelta;

    public ScrollEvent(EventSystem eventSystem, float scrollDelta) : base(eventSystem)
    {
        this.scrollDelta = scrollDelta;
    }
}
