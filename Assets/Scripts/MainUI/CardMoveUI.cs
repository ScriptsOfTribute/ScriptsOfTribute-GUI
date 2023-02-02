using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ScriptsOfTribute.Board.Cards;

public class CardMoveUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UniqueCard SingleCard { get; set; }
    public UniqueCard TargetCard { get; set; }
    public UniqueCard SourceCard { get; set; }
    public GameObject CardPrefab;
    public GameObject CardHolder;
    public GameObject TargetCardHolder;
    public GameObject SourceCardHolder;
    GameObject SingleVisibleCard;
    GameObject TargetVisibleCard;
    GameObject SourceVisibleCard;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(SingleCard != null)
        {
            SingleVisibleCard = Instantiate(CardPrefab, CardHolder.transform);
            SingleVisibleCard.transform.localScale *= 2;
            SingleVisibleCard.GetComponent<CardUIButtonScript>().SetUpCardInfo(SingleCard);
            SingleVisibleCard.GetComponent<Button>().enabled = false;
        }

        if (TargetCard != null && SourceCard != null)
        {
            TargetVisibleCard = Instantiate(CardPrefab, TargetCardHolder.transform);
            TargetVisibleCard.transform.localScale *= 1.7f;
            TargetVisibleCard.GetComponent<CardUIButtonScript>().SetUpCardInfo(TargetCard);
            TargetVisibleCard.GetComponent<Button>().enabled = false;

            SourceVisibleCard = Instantiate(CardPrefab, SourceCardHolder.transform);
            SourceVisibleCard.transform.localScale *= 1.7f;
            SourceVisibleCard.GetComponent<CardUIButtonScript>().SetUpCardInfo(SourceCard);
            SourceVisibleCard.GetComponent<Button>().enabled = false;
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (SingleCard != null)
            Destroy(SingleVisibleCard);
        if (TargetCard != null)
            Destroy(TargetVisibleCard);
        if (SourceCard != null)
            Destroy(SourceVisibleCard);
    }
}
