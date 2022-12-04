using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TalesOfTribute;
using UnityEngine.UI;

public class CardShowUIScript : MonoBehaviour
{
    public Card[] cards;

    public GameObject CardSlots;
    public GameObject cardPrefab;
    void OnEnable()
    {
        int limit = Mathf.Min(cards.Length, CardSlots.transform.childCount);
        for (int i = 0; i < limit; i++)
        {
            GameObject c = Instantiate(cardPrefab, CardSlots.transform.GetChild(i).transform);
            c.GetComponent<Button>().enabled = false;
            c.GetComponent<CardUIButtonScript>().SetUpCardInfo(cards[i]);
        }
    }

    void OnDisable()
    {
        int limit = Mathf.Min(cards.Length, CardSlots.transform.childCount);
        for (int i = 0; i < limit; i++)
        {
            Destroy(CardSlots.transform.GetChild(i).transform.GetChild(0).transform.gameObject);
        }
        cards = new Card[] { };
    }


}
