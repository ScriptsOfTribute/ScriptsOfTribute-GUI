using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TalesOfTribute;
using TalesOfTribute.Board;
using TalesOfTribute.Board.Cards;
using TalesOfTribute.Serializers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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

    public TextMeshProUGUI[] PlayerScore;
    public TextMeshProUGUI[] BotScore;

    public GameObject CardChoiceUI;
    public GameObject EffectChoiceUI;
    public GameObject EndGameUI;
    public GameObject MoveText;
    public GameObject ErrorTextField;
    public GameObject BotName;

    public static bool isUIActive = false;
    public static bool isBotPlaying = false;
    void Start()
    {
        BotName.GetComponent<TextMeshProUGUI>().SetText(TalesOfTributeAI.Instance.Name);
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
            var patronID = patronObject.transform.GetChild(0).gameObject.GetComponent<PatronScript>().patronID;
            if (patronID != PatronId.TREASURY)
            {
                var arrow = patronObject.transform.GetChild(1);
                var favor = board.PatronStates.GetFor(patronID);
                if (favor == PlayerScript.Instance.playerID)
                {
                    arrow.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                    arrow.transform.localPosition = new Vector3(0, -0.72f, 0);
                }
                else if (favor == PlayerEnum.NO_PLAYER_SELECTED)
                {
                    arrow.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    arrow.transform.localPosition = new Vector3(-0.72f, 0, 0);
                }
                else if (favor == TalesOfTributeAI.Instance.botID)
                {
                    arrow.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                    arrow.transform.localPosition = new Vector3(0, 0.72f, 0);
                }
            }
            if (patronCalls > 0 && Board.CanPatronBeActivated(patronID))
            {
                patronObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                patronObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
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

        PlayerScore[0].SetText(player.Coins.ToString());
        PlayerScore[1].SetText(player.Prestige.ToString());
        PlayerScore[2].SetText(player.Power.ToString());

        player = board.CurrentPlayer.PlayerID == TalesOfTributeAI.Instance.botID ? board.CurrentPlayer : board.EnemyPlayer;

        BotScore[0].SetText(player.Coins.ToString());
        BotScore[1].SetText(player.Prestige.ToString());
        BotScore[2].SetText(player.Power.ToString());
    }

    void SetUpTavern(SerializedBoard board)
    {
        CleanupTavern();
        List<UniqueCard> cards = board.TavernAvailableCards;
        for (int i = 0; i < Tavern.transform.childCount; i++)
        {
            GameObject card = Instantiate(CardPrefab, Tavern.transform.GetChild(i));
            card.gameObject.tag = "TavernCard";
            card.GetComponent<CardScript>().SetUpCardInfo(cards[i], cards[i].Cost <= board.CurrentPlayer.Coins);
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
        var (currentPlayer, enemyPlayer) = board.CurrentPlayer.PlayerID == PlayerScript.Instance.playerID
            ? (Player1, Player2)
            : (Player2, Player1);
        RefreshHandForPlayer(currentPlayer, board.CurrentPlayer, currentPlayer.transform.GetChild(0));
        RefreshHandForPlayer(enemyPlayer, board.EnemyPlayer, enemyPlayer.transform.GetChild(0));
    }

    void RefreshHandForPlayer(GameObject playerObject, SerializedPlayer player, Transform currentPlayerHandSlots)
    {
        for (int i = 0; i < playerObject.transform.GetChild(0).childCount; i++)
        {
            if (playerObject.transform.GetChild(0).GetChild(i).childCount > 0)
                Destroy(playerObject.transform.GetChild(0).GetChild(i).GetChild(0).gameObject);
        }

        List<UniqueCard> currentPlayerHand = player.Hand;
        var position = currentPlayerHandSlots.position;
        position = new Vector3(
            0.375f*(5 - currentPlayerHand.Count), 
            position.y, 
            position.z
        );
        currentPlayerHandSlots.position = position;
        for (int i = 0; i < currentPlayerHand.Count; i++)
        {
            GameObject card = Instantiate(CardPrefab, currentPlayerHandSlots.GetChild(i));
            card.transform.position += new Vector3(0, 0, currentPlayerHand.Count - i);
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
        bool canAfford = CardObject.GetComponent<CardScript>().CanPlayerAfford();
        if (canAfford)
        {
            Board.BuyCard(card);
        }
        else
        {
           StartCoroutine(Messages.ShowMessage(ErrorTextField, "You can't afford this card", 2));
        }
        
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
            try
            {
                Board.ActivateAgent(agent.RepresentingCard);
            }
            catch (Exception e)
            {
                StartCoroutine(Messages.ShowMessage(ErrorTextField, e.Message, 2));
            }
        }
        else if (owner == Board.EnemyPlayerId)
        {
            var agentCard = AgentObject.GetComponent<AgentScript>().GetAgent().RepresentingCard;
            var result = Board.AttackAgent(agentCard);
            if (result != null)
                EndGame(result);
        }
        yield return null;
    }

    IEnumerator HandleChoice()
    {
        SerializedChoice? pendingChoice = Board.PendingChoice;
        while (pendingChoice != null)
        {
            yield return StartCoroutine(HandleSingleChoice(pendingChoice));
            pendingChoice = Board.PendingChoice;
            yield return null;
            RefreshBoard();
        }
        yield return null;
    }

    IEnumerator HandleSingleChoice(SerializedChoice choice)
    {
        isUIActive = true;
        if (choice.Type == Choice.DataType.CARD)
        {
            CardChoiceUI.SetActive(true);
            CardChoiceUI.GetComponent<CardChoiceUIScript>().SetUpChoices(choice);
            yield return new WaitUntil(() => CardChoiceUI.GetComponent<CardChoiceUIScript>().GetCompletedStatus());
            CardChoiceUI.SetActive(false);
        }
        else if (choice.Type == Choice.DataType.EFFECT)
        {
            EffectChoiceUI.SetActive(true);
            EffectChoiceUI.GetComponent<EffectUIScript>().SetUpChoices(choice);
            yield return new WaitUntil(() => EffectChoiceUI.GetComponent<EffectUIScript>().GetCompletedStatus());
            EffectChoiceUI.SetActive(false);
        }
        isUIActive = false;
        yield return null;
    }


    public async void PlayBotMove()
    {
        var move = await TalesOfTributeAI.Instance.Play(Board.GetSerializer(), Board.GetListOfPossibleMoves());
        if (move == null)
        {
            EndGame(new EndGameState(PlayerScript.Instance.playerID, GameEndReason.MOVE_TIMEOUT, "Bot didn't move in time!"));
            return;
        }
        if (move.Command != CommandEnum.END_TURN)
            MoveText.GetComponent<TextMeshProUGUI>().text = MovesHistoryUI.ParseMove(move);
        else
            MoveText.GetComponent<TextMeshProUGUI>().text = "End turn";
        
        MoveBot(move);
    }

    public async void PlayBotAllTurnMoves()
    {
        Move move;
        do
        {
            move = await TalesOfTributeAI.Instance.Play(Board.GetSerializer(), Board.GetListOfPossibleMoves());
            if (move == null)
            {
                EndGame(new EndGameState(PlayerScript.Instance.playerID, GameEndReason.MOVE_TIMEOUT, "Bot didn't move in time!"));
                return;
            }
            MoveBot(move);
        } while (move.Command != CommandEnum.END_TURN);
        
    }

    void MoveBot(Move move)
    {
        ParseMove(move);
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
            var result = Board.PlayCard(m.Card);
            if (result != null)
                EndGame(result);
        }
        else if (move.Command == CommandEnum.ATTACK)
        {
            var m = move as SimpleCardMove;
            var result = Board.AttackAgent(m.Card);
            if (result != null)
                EndGame(result);
        }
        else if (move.Command == CommandEnum.BUY_CARD)
        {
            var m = move as SimpleCardMove;
            var result = Board.BuyCard(m.Card);
            if (result != null)
                EndGame(result);
        }
        else if (move.Command == CommandEnum.ACTIVATE_AGENT)
        {
            var m = move as SimpleCardMove;
            var result = Board.ActivateAgent(m.Card);
            if (result != null)
                EndGame(result);
        }
        else if (move.Command == CommandEnum.CALL_PATRON)
        {
            var m = move as SimplePatronMove;
            var result = Board.PatronActivation(m.PatronId);
            if (result != null)
                EndGame(result);
        }
        else if (move.Command == CommandEnum.MAKE_CHOICE)
        {
            if (move is MakeChoiceMove<UniqueCard> cardChoice)
            {
                var result = Board.MakeChoice(cardChoice.Choices);
                if (result != null)
                    EndGame(result);
            }
            else if (move is MakeChoiceMove<UniqueEffect> effectChoice)
            {
                var result = Board.MakeChoice(effectChoice.Choices.First());
                if (result != null)
                    EndGame(result);
            }
        }
        RefreshBoard();
    }

    

}
