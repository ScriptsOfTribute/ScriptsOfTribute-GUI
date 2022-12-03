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

    public GameObject CardChoiceUI;
    public GameObject EffectChoiceUI;

    private BoardState state = BoardState.NORMAL;

    void Start()
    {
        PatronId[] patrons = PatronSelectionScript.selectedPatrons.ToArray();
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
        if (Input.GetMouseButtonDown(0) && state == BoardState.NORMAL)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                var tag = hit.collider.gameObject.tag;
                Debug.Log(hit.collider.gameObject);
                switch (tag)
                {
                    case "Card":
                        StartCoroutine(PlayCard(hit.collider.gameObject));
                        break;
                    case "Patron":
                        StartCoroutine(PatronActivation(hit.collider.gameObject));
                        break;
                    case "TavernCard":
                        StartCoroutine(BuyCard(hit.collider.gameObject));
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

    void RefreshAgents()
    {
        BoardSerializer serialize = Board.GetSerializer();
        List<Card> currentPlayerAgents = Board.GetListOfAgents(serialize.CurrentPlayer);
        Transform currentPlayerAgentsSlots = serialize.CurrentPlayer == PlayerEnum.PLAYER1
                                    ? Player1.transform.GetChild(1) //0th idx is Hand, 1st is Agents
                                    : Player2.transform.GetChild(1);

        for (int i = 0; i < currentPlayerAgents.Count; i++)
        {
            GameObject card = Instantiate(CardPrefab, currentPlayerAgentsSlots.GetChild(i));
            card.GetComponent<CardScript>().SetUpCardInfo(currentPlayerAgents[i]);
        }
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

    void RefreshHand()
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
        List<Card> currentPlayerHand = Board.GetHand();
        for (int i = 0; i < currentPlayerHand.Count; i++)
        {
            GameObject card = Instantiate(CardPrefab, currentPlayerHandSlots.GetChild(i));
            card.GetComponent<CardScript>().SetUpCardInfo(currentPlayerHand[i]);
        }
    }

    IEnumerator PlayCard(GameObject card)
    {
        Debug.Log(card.GetComponent<CardScript>().GetCard());
        var chain = Board.PlayCard(card.GetComponent<CardScript>().GetCard());
        yield return StartCoroutine(ConsumeChain(chain));
        Destroy(card);

        CleanupTavern();
        SetUpTavern();

        //RefreshAgents();
        RefreshScores();
        RefreshHand();


        yield return null;
    }

    IEnumerator BuyCard(GameObject card)
    {
        Debug.Log(card.GetComponent<CardScript>().GetCard());
        var chain = Board.BuyCard(card.GetComponent<CardScript>().GetCard());
        yield return StartCoroutine(ConsumeChain(chain));
        Destroy(card);

        CleanupTavern();
        SetUpTavern();

        //RefreshAgents();
        RefreshScores();
        RefreshHand();

        yield return null;
    }

    IEnumerator PatronActivation(GameObject patronObject)
    {
        var result = Board.PatronActivation(patronObject.GetComponent<PatronScript>().patronID);
        if (result is Success)
        {
            // All fine
        }
        else if (result is Failure failure)
        {
            Debug.Log(failure.Reason);
        }
        else if (result is Choice<Card> choice)
        {
            state = BoardState.CHOICE_PENDING;
            Debug.Log("choice start");
            CardChoiceUI.SetActive(true);
            CardChoiceUI.GetComponent<CardChoiceUIScript>().SetUpChoices(choice);
            yield return new WaitUntil(() => CardChoiceUI.GetComponent<CardChoiceUIScript>().GetCompletedStatus());
            Debug.Log("choice finished");
            CardChoiceUI.SetActive(false);
        }
        else if (result is Choice<EffectType> effectChoice)
        {
            state = BoardState.CHOICE_PENDING;
            Debug.Log("choice start");
            EffectChoiceUI.SetActive(true);
            EffectChoiceUI.GetComponent<EffectUIScript>().SetUpChoices(effectChoice);
            yield return new WaitUntil(() => EffectChoiceUI.GetComponent<EffectUIScript>().GetCompletedStatus());
            EffectChoiceUI.SetActive(false);
        }
        state = BoardState.NORMAL;
        CleanupTavern();
        SetUpTavern();

        //RefreshAgents();
        RefreshScores();
        RefreshHand();

        yield return null;
    }

    IEnumerator ConsumeChain(ExecutionChain chain)
    {
        foreach (var result in chain.Consume())
        {
            if (result is Success)
            {
                continue; // All fine
            }
            else if (result is Failure failure)
            {
                Debug.Log(failure.Reason);
            }
            else if (result is Choice<Card> choice)
            {
                state = BoardState.CHOICE_PENDING;
                Debug.Log("choice start");
                CardChoiceUI.SetActive(true);
                CardChoiceUI.GetComponent<CardChoiceUIScript>().SetUpChoices(choice);
                yield return new WaitUntil(() => CardChoiceUI.GetComponent<CardChoiceUIScript>().GetCompletedStatus());
                Debug.Log("choice finished");
                CardChoiceUI.SetActive(false);
            }
            else if (result is Choice<EffectType> effectChoice)
            {
                state = BoardState.CHOICE_PENDING;
                Debug.Log("choice start");
                EffectChoiceUI.SetActive(true);
                EffectChoiceUI.GetComponent<EffectUIScript>().SetUpChoices(effectChoice);
                yield return new WaitUntil(() => EffectChoiceUI.GetComponent<EffectUIScript>().GetCompletedStatus());
                EffectChoiceUI.SetActive(false);
            }

        }
        state = BoardState.NORMAL;
        yield return null;
    }
}
