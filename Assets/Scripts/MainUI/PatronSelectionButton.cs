using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PatronSelectionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale *= 1.5f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale /= 1.5f;
    }
    public void OnClick()
    {
        FindObjectOfType<PatronSelectionScript>().PatronClicked(this.gameObject);
        this.GetComponent<Button>().enabled = false;
    }
}
