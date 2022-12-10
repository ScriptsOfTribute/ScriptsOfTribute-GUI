using System.Collections;
using System.Collections.Generic;
using TalesOfTribute;
using TalesOfTribute.Board;
using TalesOfTribute.Serializers;
using TMPro;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static TalesOfTributeApi Board;
    public GameObject Tavern;
    public GameObject Player1;
    public GameObject Player2;
    public GameObject Patrons;

    public GameObject CardPrefab;
    public GameObject AgentPrefab;
    public GameObject[] PatronsPrefabs;

    public GameObject Player1Score;
    public GameObject Player2Score;

    public GameObject CardChoiceUI;
    public GameObject EffectChoiceUI;

    private BoardState state = BoardState.NORMAL;
    public static bool isUIActive = false;

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
        if (Input.GetMouseButtonDown(0) && state == BoardState.NORMAL && !isUIActive)
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
                        StartCoroutine(PlayCard(hit.collider.gameObject));
                        break;
                    case "Patron":
                        StartCoroutine(PatronActivation(hit.collider.gameObject));
                        break;
                    case "TavernCard":
                        StartCoroutine(BuyCard(hit.collider.gameObject));
                        break;
                    case "Agent":
                        StartCoroutine(HandleAgent(hit.collider.gameObject));
                        break;
                }
                RefreshScores();
                RefreshAgents();
            }
        }
    }

    public void StartTurn()
    {
        SerializedBoard serialize = Board.GetSerializer();
        List<Card> currentPlayerHand = Board.GetHand();
        Transform currentPlayerHandSlots = serialize.CurrentPlayer.PlayerID == PlayerEnum.PLAYER1
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
        SerializedBoard serialize = Board.GetSerializer();
        Transform currentPlayerHandSlots = serialize.CurrentPlayer.PlayerID == PlayerEnum.PLAYER1
                                    ? Player1.transform.GetChild(0) //0th idx is Hand, 1st is Agents
                                    : Player2.transform.GetChild(0);

        for (int i = 0; i < currentPlayerHandSlots.childCount; i++)
        {
            if (currentPlayerHandSlots.GetChild(i).childCount > 0)
                Destroy(currentPlayerHandSlots.GetChild(i).GetChild(0).gameObject);
        }
        Board.EndTurn();
        RefreshScores();
        RefreshAgents();
        StartTurn();
    }

    void RefreshAgents()
    {
        SerializedBoard serialize = Board.GetSerializer();
        List<SerializedAgent> currentPlayerAgents = serialize.CurrentPlayer.Agents;
        List<SerializedAgent> enemyPlayerAgents = serialize.EnemyPlayer.Agents;
        Transform currentPlayerAgentsSlots = serialize.CurrentPlayer.PlayerID == PlayerEnum.PLAYER1
                                    ? Player1.transform.GetChild(1) //0th idx is Hand, 1st is Agents
                                    : Player2.transform.GetChild(1);
        Transform enemyAgentsSlots = serialize.CurrentPlayer.PlayerID == PlayerEnum.PLAYER1
                                    ? Player2.transform.GetChild(1) //0th idx is Hand, 1st is Agents
                                    : Player1.transform.GetChild(1);

        for (int i = 0; i < currentPlayerAgentsSlots.childCount; i++)
        {
            if (currentPlayerAgentsSlots.GetChild(i).childCount > 0)
            {
                Destroy(currentPlayerAgentsSlots.GetChild(i).GetChild(0).gameObject);
            }                
        }

        for (int i = 0; i < enemyAgentsSlots.childCount; i++)
        {
            if (enemyAgentsSlots.GetChild(i).childCount > 0)
            {
                Destroy(enemyAgentsSlots.GetChild(i).GetChild(0).gameObject);
            }
        }

        for (int i = 0; i < currentPlayerAgents.Count; i++)
        {
            Debug.Log(currentPlayerAgents[i].RepresentingCard.ToString());
            GameObject card = Instantiate(AgentPrefab, currentPlayerAgentsSlots.GetChild(i));
            card.GetComponent<AgentScript>().SetUpCardInfo(currentPlayerAgents[i]);
        }

        for (int i = 0; i < enemyPlayerAgents.Count; i++)
        {
            Debug.Log(enemyPlayerAgents[i].RepresentingCard.ToString());
            GameObject card = Instantiate(AgentPrefab, enemyAgentsSlots.GetChild(i));
            card.GetComponent<AgentScript>().SetUpCardInfo(enemyPlayerAgents[i]);
        }
    }

    public void RefreshScores()
    {
        SerializedPlayer player = Board.GetPlayer(PlayerEnum.PLAYER1);

        string text = $"Gold: {player.Coins}\n" +
                    $"Prestige: {player.Prestige}\n" +
                    $"Power: {player.Power}\n";

        Player1Score.GetComponent<TextMeshProUGUI>().SetText(text);

        player = Board.GetPlayer(PlayerEnum.PLAYER2);

        text = $"Gold: {player.Coins}\n" +
                $"Prestige: {player.Prestige}\n" +
                $"Power: {player.Power}\n";

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
        SerializedBoard serialize = Board.GetSerializer();
        Transform currentPlayerHandSlots = serialize.CurrentPlayer.PlayerID == PlayerEnum.PLAYER1
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

        RefreshScores();
        RefreshHand();
        RefreshAgents();


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

        RefreshScores();
        RefreshHand();
        RefreshAgents();

        yield return null;
    }

    IEnumerator PatronActivation(GameObject patronObject)
    {
        Debug.Log(patronObject.GetComponent<PatronScript>().patronID);
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
        if (patronObject.GetComponent<PatronScript>().patronID != PatronId.TREASURY)
        {
            var arrow = patronObject.transform.parent.transform.GetChild(0);
            var favor = Board.GetLevelOfFavoritism(patronObject.GetComponent<PatronScript>().patronID);
            switch (favor)
            {
                case PlayerEnum.PLAYER1:
                    arrow.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
                    break;
                case PlayerEnum.NO_PLAYER_SELECTED:
                    arrow.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                    break;
                case PlayerEnum.PLAYER2:
                    arrow.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                    break;
            }

        }
        CleanupTavern();
        SetUpTavern();

        RefreshAgents();
        RefreshScores();
        RefreshHand();

        yield return null;
    }

    IEnumerator HandleAgent(GameObject agent)
    {
        var playerTag = agent.transform.parent.parent.parent.tag;
        SerializedBoard serialize = Board.GetSerializer();
        Debug.Log(playerTag);
        Debug.Log(serialize.CurrentPlayer.PlayerID);
        if (playerTag == "Player1" && serialize.CurrentPlayer.PlayerID == PlayerEnum.PLAYER1)
        {
            var chain = Board.ActivateAgent(agent.GetComponent<AgentScript>().GetAgent().RepresentingCard);
            yield return StartCoroutine(ConsumeChain(chain));
        }
        else if (playerTag == "Player2" && serialize.CurrentPlayer.PlayerID == PlayerEnum.PLAYER1)
        {
            var result = Board.AttackAgent(agent.GetComponent<AgentScript>().GetAgent().RepresentingCard);
            if (result is Failure fail)
            {
                Debug.Log(fail.Reason);
            }
        }
        else if (playerTag == "Player2" && serialize.CurrentPlayer.PlayerID == PlayerEnum.PLAYER2)
        {
            var chain = Board.ActivateAgent(agent.GetComponent<AgentScript>().GetAgent().RepresentingCard);
            yield return StartCoroutine(ConsumeChain(chain));
        }
        else if (playerTag == "Player1" && serialize.CurrentPlayer.PlayerID == PlayerEnum.PLAYER2)
        {
            var result = Board.AttackAgent(agent.GetComponent<AgentScript>().GetAgent().RepresentingCard);
            if (result is Failure fail)
            {
                Debug.Log(fail.Reason);
            }
        }

        CleanupTavern();
        SetUpTavern();

        RefreshAgents();
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
