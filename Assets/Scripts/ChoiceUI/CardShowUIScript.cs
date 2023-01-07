using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TalesOfTribute;
using UnityEngine.UI;
using TalesOfTribute.Board.Cards;
using TMPro;

public class CardShowUIScript : MonoBehaviour
{
    public UniqueCard[] cards;

    public GameObject Container;
    public GameObject cardPrefab;
    public TextMeshProUGUI title;
    void OnEnable()
    {
        GameManager.isUIActive = true;
        for (int i = 0; i < cards.Length; i++)
        {
            GameObject c = Instantiate(cardPrefab, Container.transform);
            c.GetComponent<Button>().enabled = false;
            c.GetComponent<CardUIButtonScript>().SetUpCardInfo(cards[i]);
        }
    }

    void OnDisable()
    {
        for (int i = 0; i < Container.transform.childCount; i++)
        {
            Destroy(Container.transform.GetChild(i).gameObject);
        }
        cards = new UniqueCard[] { };
        GameManager.isUIActive = false;
    }
}
