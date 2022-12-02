using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TalesOfTribute;
using TMPro;
using System;
public class GameManager : MonoBehaviour
{
    private static TalesOfTributeApi Board;
    public GameObject Tavern;
    public GameObject Player1;
    public GameObject Player2;
    public GameObject Patrons;

    public GameObject CardPrefab;
    public GameObject[] PatronsPrefabs;

    public GameObject Player1Score;
    public GameObject Player2Score;

    void Start()
    {
        PatronId[] patrons = new PatronId[5]
        {
            PatronId.ANSEI,
            PatronId.DUKE_OF_CROWS,
            PatronId.TREASURY,
            PatronId.ORGNUM,
            PatronId.RAJHIN
        };
        Board = new TalesOfTributeApi(patrons);

        for (int i = 0; i < Patrons.transform.childCount; i++)
        {
            var slot = Patrons.transform.GetChild(i);
            Instantiate(PatronsPrefabs[(int)patrons[i]], slot);
        }

        SetUpTavern();
        StartTurn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                var tag = hit.collider.gameObject.tag;
                switch (tag)
                {
                    case "Card":
                        PlayCard(hit.collider.gameObject);
                        break;
                    case "Patron":
                        PatronActivation(hit.collider.gameObject.GetComponentInParent<PatronScript>().patronID);
                        break;
                    case "TavernCard":
                        BuyCard(hit.collider.gameObject);
                        break;
                }
            }
        }
    }

    public void StartTurn()
    {
        Board.DrawCards();
        BoardSerializer serialize = Board.GetSerializer();
        List<Card> currentPlayerHand = Board.GetHand();
        Transform currentPlayerHandSlots = serialize.CurrentPlayer == PlayerEnum.PLAYER1
                                    ? Player1.transform.GetChild(0) //0th idx is Hand, 1st is Agents
                                    : Player2.transform.GetChild(0);

        for (int i = 0; i < currentPlayerHandSlots.childCount; i++)
        {
            GameObject card = Instantiate(CardPrefab, currentPlayerHandSlots.GetChild(i));
            card.GetComponent<CardScript>().SetUpCardInfo(currentPlayerHand[i]);
        }
        RefreshScores();
    }

    public void EndTurn()
    {
        BoardSerializer serialize = Board.GetSerializer();
        Transform currentPlayerHandSlots = serialize.CurrentPlayer == PlayerEnum.PLAYER1
                                    ? Player1.transform.GetChild(0) //0th idx is Hand, 1st is Agents
                                    : Player2.transform.GetChild(0);

        for (int i = 0; i < currentPlayerHandSlots.childCount; i++)
        {
            if (currentPlayerHandSlots.GetChild(i).childCount > 0)
                Destroy(currentPlayerHandSlots.GetChild(i).GetChild(0).gameObject);
        }
        Board.EndTurn();
        RefreshScores();
        StartTurn();
    }

    public void RefreshScores()
    {
        PlayerSerializer serialize = Board.GetPlayersScores();

        string text = $"Gold: {serialize.FirstPlayer.X}\n" +
                    $"Prestige: {serialize.FirstPlayer.Y}\n" +
                    $"Power: {serialize.FirstPlayer.Z}\n";

        Player1Score.GetComponent<TextMeshProUGUI>().SetText(text);

        text = $"Gold: {serialize.SecondPlayer.X}\n" +
                    $"Prestige: {serialize.SecondPlayer.Y}\n" +
                    $"Power: {serialize.SecondPlayer.Z}\n";

        Player2Score.GetComponent<TextMeshProUGUI>().SetText(text);
    }

    void SetUpTavern()
    {
        List<Card> cards = Board.GetTavern();
        for (int i = 0; i < Tavern.transform.childCount; i++)
        {
            GameObject card = Instantiate(CardPrefab, Tavern.transform.GetChild(i));
            card.gameObject.tag = "TavernCard";
            card.GetComponent<CardScript>().SetUpCardInfo(cards[i]);
        }
    }

    void CleanupTavern()
    {
        for (int i = 0; i < Tavern.transform.childCount; i++)
        {
            if (Tavern.transform.GetChild(i).childCount > 0)
                Destroy(Tavern.transform.GetChild(i).GetChild(0).gameObject);
        }
    }

    void PlayCard(GameObject card)
    {
        try
        {
            var chain = Board.PlayCard(card.GetComponent<CardScript>().GetCard());
            foreach (PlayResult result in chain.Consume())
            {

            }
            Destroy(card);
            RefreshScores();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            return;
        }
    }

    void BuyCard(GameObject card)
    {
        try
        {
            var chain = Board.BuyCard(card.GetComponent<CardScript>().GetCard());
            foreach (PlayResult result in chain.Consume())
            {

            }
            Destroy(card);

            CleanupTavern();
            SetUpTavern();

            RefreshScores();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            return;
        }
    }

    void PatronActivation(PatronId patron)
    {
        try
        {
            var result = Board.PatronActivation(patron);
            if (!result.Completed)
            {
                Debug.Log("kaput");
            }
            RefreshScores();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            return;
        }
    }
}
