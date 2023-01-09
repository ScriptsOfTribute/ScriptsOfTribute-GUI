using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PatronSelectionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI FavoredText;
    public TextMeshProUGUI NeutralText;
    public TextMeshProUGUI UnFavoredText;
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale *= 1.5f;
        NameText.SetText(GetComponent<PatronScript>().GetName());
        FavoredText.SetText(GetComponent<PatronScript>().GetFavoredDescription());
        NeutralText.SetText(GetComponent<PatronScript>().GetNeutralDescription());
        UnFavoredText.SetText(GetComponent<PatronScript>().GetUnFavoredDescription());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale /= 1.5f;
        NameText.SetText("");
        FavoredText.SetText("");
        NeutralText.SetText("");
        UnFavoredText.SetText("");
    }
    public void OnClick()
    {
        FindObjectOfType<PatronSelectionScript>().PatronClicked(this.gameObject);
        this.GetComponent<Button>().enabled = false;
    }
}
