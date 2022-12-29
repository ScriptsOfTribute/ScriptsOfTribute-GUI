using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public GameObject EndGameUI;
    public GameObject MoveText;

    public static bool isUIActive = false;
    public static bool isBotPlaying = false;

    void Start()
    {
        PatronId[] patrons = PatronSelectionScript.selectedPatrons.ToArray();
        Board = new TalesOfTributeApi(patrons);

        for (int i = 0; i < Patrons.transform.childCount; i++)
        {
            var slot = Patrons.transform.GetChild(i);
            Instantiate(PatronsPrefabs[(int)patrons[i]], slot);
        }
        RefreshBoard();
        StartTurn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isUIActive && PlayerScript.Instance.playerID == Board.CurrentPlayerId)
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
            }
            StartCoroutine(HandleChoice());
        }
    }

    void RefreshBoard()
    {
        SerializedBoard board = Board.GetSerializer();
        uint patronCalls = board.CurrentPlayer.PatronCalls;
        for(int i = 0; i < Patrons.transform.childCount; i++)
        {
            var patronObject = Patrons.transform.GetChild(i).GetChild(0).gameObject;
            if (patronObject.transform.childCount > 1) // Only treasury has one bcs it has no arrow
            {
                patronObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = patronCalls == 0 ? Color.gray : Color.white;

                var patronID = patronObject.transform.GetChild(1).gameObject.GetComponent<PatronScript>().patronID;
                var arrow = patronObject.transform.GetChild(0);
                var favor = board.PatronStates.GetFor(patronID);
                if (favor == PlayerScript.Instance.playerID)
                        arrow.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
                else if (favor == PlayerEnum.NO_PLAYER_SELECTED)
                        arrow.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                else if (favor == TalesOfTributeAI.Instance.botID)
                    arrow.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            }
            else if (patronObject.transform.childCount == 1)
            {
                patronObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = patronCalls == 0 ? Color.gray : Color.white;
            }
        }

        RefreshAgents(board);
        RefreshScores(board);
        RefreshHand(board);
        SetUpTavern(board);
    }

    public void StartTurn()
    {
        isBotPlaying = Board.CurrentPlayerId != PlayerScript.Instance.playerID;
        if (Board.PendingChoice != null && !isBotPlaying)
        {
            StartCoroutine(HandleChoice());
        }
        
    }

    public void EndTurn()
    {
        Board.EndTurn();
#nullable enable
        EndGameState? endGame = Board.CheckWinner();
        if (endGame != null)
            EndGame(endGame);
#nullable disable
        RefreshBoard();
        StartTurn();
    }

    void EndGame(EndGameState state)
    {
        EndGameUI.SetActive(true);
        EndGameUI.GetComponent<EndGameUI>().SetUp(state);
        this.enabled = false;
    }

    void RefreshAgents(SerializedBoard board)
    {
        List<SerializedAgent> currentPlayerAgents = board.CurrentPlayer.Agents;
        List<SerializedAgent> enemyPlayerAgents = board.EnemyPlayer.Agents;
        Transform currentPlayerAgentsSlots = board.CurrentPlayer.PlayerID == PlayerScript.Instance.playerID
                                    ? Player1.transform.GetChild(1) //0th idx is Hand, 1st is Agents
                                    : Player2.transform.GetChild(1);
        Transform enemyAgentsSlots = board.CurrentPlayer.PlayerID == PlayerScript.Instance.playerID
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
            GameObject card = Instantiate(AgentPrefab, currentPlayerAgentsSlots.GetChild(i));
            card.GetComponent<AgentScript>().SetUpCardInfo(currentPlayerAgents[i], board.CurrentPlayer.PlayerID);
        }

        for (int i = 0; i < enemyPlayerAgents.Count; i++)
        {
            GameObject card = Instantiate(AgentPrefab, enemyAgentsSlots.GetChild(i));
            card.GetComponent<AgentScript>().SetUpCardInfo(enemyPlayerAgents[i], board.EnemyPlayer.PlayerID);
        }
    }

    public void RefreshScores(SerializedBoard board)
    {
        SerializedPlayer player = board.CurrentPlayer.PlayerID == PlayerScript.Instance.playerID ? board.CurrentPlayer : board.EnemyPlayer;

        string text = $"Gold: {player.Coins}\n" +
                    $"Prestige: {player.Prestige}\n" +
                    $"Power: {player.Power}\n";

        Player1Score.GetComponent<TextMeshProUGUI>().SetText(text);

        player = board.CurrentPlayer.PlayerID == TalesOfTributeAI.Instance.botID ? board.CurrentPlayer : board.EnemyPlayer;

        text = $"Gold: {player.Coins}\n" +
                $"Prestige: {player.Prestige}\n" +
                $"Power: {player.Power}\n";

        Player2Score.GetComponent<TextMeshProUGUI>().SetText(text);
    }

    void SetUpTavern(SerializedBoard board)
    {
        CleanupTavern();
        List<Card> cards = board.TavernAvailableCards;
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

    void RefreshHand(SerializedBoard board)
    {
        Transform currentPlayerHandSlots = board.CurrentPlayer.PlayerID == PlayerScript.Instance.playerID
                                    ? Player1.transform.GetChild(0) //0th idx is Hand, 1st is Agents
                                    : Player2.transform.GetChild(0);

        for (int i = 0; i < Player1.transform.GetChild(0).childCount; i++)
        {
            if (Player1.transform.GetChild(0).GetChild(i).childCount > 0)
                Destroy(Player1.transform.GetChild(0).GetChild(i).GetChild(0).gameObject);
        }

        for (int i = 0; i < Player2.transform.GetChild(0).childCount; i++)
        {
            if (Player2.transform.GetChild(0).GetChild(i).childCount > 0)
                Destroy(Player2.transform.GetChild(0).GetChild(i).GetChild(0).gameObject);
        }
        List<Card> currentPlayerHand = board.CurrentPlayer.Hand;
        for (int i = 0; i < currentPlayerHand.Count; i++)
        {
            GameObject card = Instantiate(CardPrefab, currentPlayerHandSlots.GetChild(i));
            card.GetComponent<CardScript>().SetUpCardInfo(currentPlayerHand[i]);
        }
    }

    IEnumerator PlayCard(GameObject CardObject)
    {
        var card = CardObject.GetComponent<CardScript>().GetCard();
        Board.PlayCard(card);
        RefreshBoard();
        yield return null;
    }

    IEnumerator BuyCard(GameObject CardObject)
    {
        var card = CardObject.GetComponent<CardScript>().GetCard();
        Board.BuyCard(card);
        RefreshBoard();
        yield return null;
    }

    IEnumerator PatronActivation(GameObject PatronObject)
    {
        var patronID = PatronObject.GetComponent<PatronScript>().patronID;
        Board.PatronActivation(patronID);
        RefreshBoard();
        yield return null;
    }

    IEnumerator HandleAgent(GameObject AgentObject)
    {
        var agent = AgentObject.GetComponent<AgentScript>().GetAgent();
        var owner = AgentObject.GetComponent<AgentScript>().GetOwner();
        if (owner == Board.CurrentPlayerId)
        {
            Board.ActivateAgent(agent.RepresentingCard);
        }
        else if (owner == Board.EnemyPlayerId)
        {
            var result = Board.AttackAgent(AgentObject.GetComponent<AgentScript>().GetAgent().RepresentingCard);
            if (result is Failure fail)
            {
                Debug.Log(fail.Reason);
            }
        }
        yield return null;
    }

    IEnumerator HandleChoice()
    {
        BaseSerializedChoice? pendingChoice = Board.PendingChoice;
        while (pendingChoice != null)
        {
            yield return StartCoroutine(HandleSingleChoice(pendingChoice.ToChoice()));
            pendingChoice = Board.PendingChoice;
        }

        yield return null;
        RefreshBoard();
        yield return null;
    }

    IEnumerator HandleSingleChoice(BaseChoice choice)
    {
        isUIActive = true;
        if (choice is Choice<Card> cardChoice)
        {
            CardChoiceUI.SetActive(true);
            CardChoiceUI.GetComponent<CardChoiceUIScript>().SetUpChoices(cardChoice);
            yield return new WaitUntil(() => CardChoiceUI.GetComponent<CardChoiceUIScript>().GetCompletedStatus());
            CardChoiceUI.SetActive(false);
        }
        else if (choice is Choice<Effect> effectChoice)
        {
            EffectChoiceUI.SetActive(true);
            EffectChoiceUI.GetComponent<EffectUIScript>().SetUpChoices(effectChoice);
            yield return new WaitUntil(() => EffectChoiceUI.GetComponent<EffectUIScript>().GetCompletedStatus());
            EffectChoiceUI.SetActive(false);
        }
        isUIActive = false;
        yield return null;
    }


    public IEnumerator PlayBotMove()
    {
        var move = TalesOfTributeAI.Instance.Play(Board.GetSerializer(), Board.GetListOfPossibleMoves());
        MoveText.GetComponent<TextMeshProUGUI>().text = TalesOfTributeAI.Instance.GetMoves().FindLast(s => !s.StartsWith("--"));
        yield return StartCoroutine(MoveBot(move));
    }

    IEnumerator MoveBot(Move move)
    {
        ParseMove(move);
        yield return null;
    }

    void ParseMove(Move move)
    {
        if (move.Command == CommandEnum.END_TURN)
        {
            EndTurn();
            return;
        }
        else if (move.Command == CommandEnum.PLAY_CARD)
        {
            var m = move as SimpleCardMove;
            Board.PlayCard(m.Card);
        }
        else if (move.Command == CommandEnum.ATTACK)
        {
            var m = move as SimpleCardMove;
            var result = Board.AttackAgent(m.Card);
            if (result is Failure f)
            {
                EndGame(new EndGameState(Board.EnemyPlayerId, GameEndReason.INCORRECT_MOVE, f.Reason));
            }
        }
        else if (move.Command == CommandEnum.BUY_CARD)
        {
            var m = move as SimpleCardMove;
            Board.BuyCard(m.Card);
        }
        else if (move.Command == CommandEnum.ACTIVATE_AGENT)
        {
            var m = move as SimpleCardMove;
            Board.ActivateAgent(m.Card);
        }
        else if (move.Command == CommandEnum.CALL_PATRON)
        {
            var m = move as SimplePatronMove;
            Board.PatronActivation(m.PatronId);
        }
        else if (move.Command == CommandEnum.MAKE_CHOICE)
        {
            if (move is MakeChoiceMove<Card> cardChoice)
            {
                Board.MakeChoice(cardChoice.Choices);
            }
            else if (move is MakeChoiceMove<Effect> effectChoice)
            {
                Board.MakeChoice(effectChoice.Choices);
            }
        }

        RefreshBoard();
    }

}
