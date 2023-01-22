using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TalesOfTribute;
using TalesOfTribute.Board;
using TalesOfTribute.Board.Cards;
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

    public TextMeshProUGUI[] PlayerScore;
    public TextMeshProUGUI[] BotScore;

    public GameObject CardChoiceUI;
    public GameObject EffectChoiceUI;
    public GameObject EndGameUI;
    public GameObject MoveText;
    public GameObject ErrorTextField;
    public GameObject BotName;
    public GameObject BlackFade;

    public AudioSource cardAudio;
    public AudioSource buyCardAudio;

    public static bool isUIActive = false;
    public static bool isBotPlaying = false;
    private Coroutine _botTextCoroutine = null;

    private int _PlayerPrestigeStart;
    private int _BotPrestigeStart;
    private List<GameObject> _trackedObjects = new List<GameObject>();
    void Start()
    {
        BotName.GetComponent<TextMeshProUGUI>().SetText(TalesOfTributeAI.Instance.Name);
        PatronId[] patrons = PatronSelectionScript.selectedPatrons.ToArray();
        var seed = BoardManager.Instance.GetSeed();
        
        if (seed != 0)
        {
            Board = new TalesOfTributeApi(patrons, seed);
        }
        else
        {
            Board = new TalesOfTributeApi(patrons);
            BoardManager.Instance.SetSeed(Board.Seed);
        }
        if(TalesOfTributeAI.Instance.botID == PlayerEnum.PLAYER1)
        {
            Board.Logger.P1LoggerEnabled = true;
            Board.Logger.P1LogTarget = new UnityLogStream();
        } else if (TalesOfTributeAI.Instance.botID == PlayerEnum.PLAYER2)
        {
            Board.Logger.P2LoggerEnabled = true;
            Board.Logger.P2LogTarget = new UnityLogStream();
        }
        
        for (int i = 0; i < Patrons.transform.childCount; i++)
        {
            var slot = Patrons.transform.GetChild(i);
            Instantiate(PatronsPrefabs[(int)patrons[i]], slot);
        }
        _PlayerPrestigeStart = 0;
        _BotPrestigeStart = 0;

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
        foreach(var gameObject in _trackedObjects)
        {
            Destroy(gameObject);
        }
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
                    arrow.GetComponent<SpriteRenderer>().color = Color.green;
                }
                else if (favor == PlayerEnum.NO_PLAYER_SELECTED)
                {
                    arrow.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    arrow.transform.localPosition = new Vector3(-0.72f, 0, 0);
                    arrow.GetComponent<SpriteRenderer>().color = Color.white;
                }
                else if (favor == TalesOfTributeAI.Instance.botID)
                {
                    arrow.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                    arrow.transform.localPosition = new Vector3(0, 0.72f, 0);
                    arrow.GetComponent<SpriteRenderer>().color = Color.red;
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

        Logger.Instance.UpdateMoves(board.CompletedActions);
        FindObjectOfType<ComboHoverUI>().Close();
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
        {
            StartCoroutine(EndGame(endGame));
        }
        else
        {
            RefreshBoard();
            StartTurn();
            
        }
#nullable disable
        SerializedBoard board = Board.GetSerializer();
        SerializedPlayer player = Board.CurrentPlayerId == PlayerScript.Instance.playerID ?
            board.CurrentPlayer : board.EnemyPlayer;

        if (player.Prestige - _PlayerPrestigeStart != 0)
        {
            var diff = (player.Prestige - _PlayerPrestigeStart).ToString();
            string text = player.Prestige - _PlayerPrestigeStart > 0 ? "+" : "";
            StartCoroutine(
                Messages.ShowMessage(PlayerScore[3].transform.gameObject, text + diff, 2)
                );
            _PlayerPrestigeStart = player.Prestige;
        }
        
        player = board.CurrentPlayer.PlayerID == TalesOfTributeAI.Instance.botID ? board.CurrentPlayer : board.EnemyPlayer;
        if (player.Prestige - _BotPrestigeStart != 0)
        {
            var diff = (player.Prestige - _BotPrestigeStart).ToString();
            string text = player.Prestige - _BotPrestigeStart > 0 ? "+" : "";
            StartCoroutine(
                Messages.ShowMessage(BotScore[3].transform.gameObject, text + diff, 2)
                );
            _BotPrestigeStart = player.Prestige;
        }
        
    }

    IEnumerator EndGame(EndGameState state)
    {
        float fadeAmount = 5f;
        Color objectColor = BlackFade.GetComponent<UnityEngine.UI.Image>().color;
        while (BlackFade.GetComponent<UnityEngine.UI.Image>().color.a < 1)
        {
            fadeAmount = objectColor.a + (1f * Time.deltaTime);
            objectColor = new Color(0, 0, 0, fadeAmount);
            BlackFade.GetComponent<UnityEngine.UI.Image>().color = objectColor;
            yield return null;
        }
        
        EndGameUI.SetActive(true);
        yield return StartCoroutine(EndGameUI.GetComponent<EndGameUI>().SetUp(state));
        transform.parent.gameObject.SetActive(false);
        yield return null;
    }

    void RefreshAgents(SerializedBoard board)
    {
        List<SerializedAgent> currentPlayerAgents = board.CurrentPlayer.Agents;
        List<SerializedAgent> enemyPlayerAgents = board.EnemyPlayer.Agents;
        ComboStates states = board.ComboStates;
        Transform currentPlayerAgentsSlots = board.CurrentPlayer.PlayerID == PlayerScript.Instance.playerID
                                    ? Player1.transform.GetChild(1) //0th idx is Hand, 1st is Agents
                                    : Player2.transform.GetChild(1);
        Transform enemyAgentsSlots = board.CurrentPlayer.PlayerID == PlayerScript.Instance.playerID
                                    ? Player2.transform.GetChild(1) //0th idx is Hand, 1st is Agents
                                    : Player1.transform.GetChild(1);


        for (int i = 0; i < currentPlayerAgents.Count; i++)
        {
            GameObject card = Instantiate(AgentPrefab, currentPlayerAgentsSlots.GetChild(i));
            _trackedObjects.Add(card);
            ComboState state;
            var isPresent = states.All.TryGetValue(currentPlayerAgents[i].RepresentingCard.Deck, out state);
            if (!isPresent)
            {
                state = new ComboState(new List<UniqueBaseEffect>[1], 0);
            }
            card.GetComponent<AgentScript>().SetUpCardInfo(currentPlayerAgents[i], state, board.CurrentPlayer.PlayerID);
        }

        for (int i = 0; i < enemyPlayerAgents.Count; i++)
        {
            GameObject card = Instantiate(AgentPrefab, enemyAgentsSlots.GetChild(i));
            _trackedObjects.Add(card);
            card.GetComponent<AgentScript>().SetUpCardInfo(enemyPlayerAgents[i], new ComboState(new List<UniqueBaseEffect>[1], 0), board.EnemyPlayer.PlayerID);
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
        List<UniqueCard> cards = board.TavernAvailableCards;
        ComboStates states = board.ComboStates;
        for (int i = 0; i < Tavern.transform.childCount; i++)
        {
            GameObject card = Instantiate(CardPrefab, Tavern.transform.GetChild(i));
            _trackedObjects.Add(card);
            card.gameObject.tag = "TavernCard";
            
            ComboState state;
            var isPresent = states.All.TryGetValue(cards[i].Deck, out state);
            if (!isPresent)
            {
                state = new ComboState(new List<UniqueBaseEffect>[1], 0);
            }
            card.GetComponent<CardScript>().SetUpCardInfo(cards[i], state, cards[i].Cost <= board.CurrentPlayer.Coins);
        }
    }

    void RefreshHand(SerializedBoard board)
    {
        Transform currentPlayerHandSlots = board.CurrentPlayer.PlayerID == PlayerScript.Instance.playerID
                                    ? Player1.transform.GetChild(0) //0th idx is Hand, 1st is Agents
                                    : Player2.transform.GetChild(0);

        List<UniqueCard> currentPlayerHand = board.CurrentPlayer.Hand;
        currentPlayerHandSlots.position = new Vector3(
            -1.2f + 0.375f*(5 - currentPlayerHand.Count), 
            currentPlayerHandSlots.position.y, 
            currentPlayerHandSlots.position.z
        );

        ComboStates states = board.ComboStates;

        for (int i = 0; i < currentPlayerHand.Count; i++)
        {
            GameObject card = Instantiate(CardPrefab, currentPlayerHandSlots.GetChild(i));
            _trackedObjects.Add(card);
            card.transform.position += new Vector3(0, 0, currentPlayerHand.Count - i);
            if (currentPlayerHandSlots.IsChildOf(Player2.transform))
            {
                card.transform.Rotate(new Vector3(0, 0, 180f));
            }
            var deck = currentPlayerHand[i].Deck;
            ComboState state;
            var isPresent = states.All.TryGetValue(deck, out state);
            if (isPresent)
            {
                card.GetComponent<CardScript>().SetUpCardInfo(currentPlayerHand[i], state);
            }
            else
            {
                card.GetComponent<CardScript>().SetUpCardInfo(currentPlayerHand[i], new ComboState(new List<UniqueBaseEffect>[1], 0));
            }
        }
    }

    IEnumerator PlayCard(GameObject CardObject)
    {
        var card = CardObject.GetComponent<CardScript>().GetCard();
        Board.PlayCard(card);
        cardAudio.Play();
        RefreshBoard();
        yield return null;
    }

    IEnumerator BuyCard(GameObject CardObject)
    {
        var card = CardObject.GetComponent<CardScript>().GetCard();
        bool canAfford = CardObject.GetComponent<CardScript>().CanPlayerAfford();
        if (canAfford)
        {
            buyCardAudio.Play();
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
        if (Board.GetSerializer().CurrentPlayer.PatronCalls > 0 && Board.CanPatronBeActivated(patronID))
        {
            Board.PatronActivation(patronID);
            RefreshBoard();
        }
        else
        {
            StartCoroutine(Messages.ShowMessage(ErrorTextField, "You can't activate this patron", 2));
        }
        yield return null;
    }

    IEnumerator HandleAgent(GameObject AgentObject)
    {
        var agent = AgentObject.GetComponent<AgentScript>().GetAgent();
        var owner = AgentObject.GetComponent<AgentScript>().GetOwner();
        if (owner == Board.CurrentPlayerId)
        {
            if(!agent.Activated)
            {
                Board.ActivateAgent(agent.RepresentingCard);
            }
            else
            {
                StartCoroutine(Messages.ShowMessage(ErrorTextField, "This agent is already activated", 2));
            }
        }
        else if (owner == Board.EnemyPlayerId)
        {
            var agentCard = AgentObject.GetComponent<AgentScript>().GetAgent().RepresentingCard;
            var result = Board.AttackAgent(agentCard);
            if (result != null)
                StartCoroutine(EndGame(result));
        }
        RefreshBoard();
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


    public IEnumerator PlayBotMove()
    {
        var thread = new Thread(() => TalesOfTributeAI.Instance.Play(new GameState(Board.GetSerializer()), Board.GetListOfPossibleMoves()));
        thread.Start();
        yield return new WaitUntil(() => !thread.IsAlive);
        var move = TalesOfTributeAI.Instance.move;
        if (move == null)
        {
            yield return StartCoroutine(EndGame(new EndGameState(PlayerScript.Instance.playerID, GameEndReason.TURN_TIMEOUT, "Bot didn't finish turn in time!")));
        }
        if (_botTextCoroutine != null)
            StopCoroutine(_botTextCoroutine);
        if (move.Command != CommandEnum.END_TURN)
            _botTextCoroutine = StartCoroutine(Messages.ShowMessage(MoveText, MovesHistoryUI.ParseMove(move), 2));
        else
            _botTextCoroutine = StartCoroutine(Messages.ShowMessage(MoveText, "End turn", 2));
        MoveBot(move);
        TalesOfTributeAI.Instance.GetLogMessages().ForEach(m => Board.Logger.Log(TalesOfTributeAI.Instance.botID, m.Item2));
        BotLogsScript.Instance.Refresh();
        Board.Logger.Flush();
        RefreshBoard();
        TalesOfTributeAI.Instance.move = null;
        yield return null;
    }

    public IEnumerator PlayBotAllTurnMoves()
    {
        Move move;
        do
        {
            var thread = new Thread(() => TalesOfTributeAI.Instance.Play(new GameState(Board.GetSerializer()), Board.GetListOfPossibleMoves()));
            thread.Start();
            yield return new WaitUntil(() => !thread.IsAlive);
            move = TalesOfTributeAI.Instance.move;
            if (move == null)
            {
                yield return StartCoroutine(EndGame(new EndGameState(PlayerScript.Instance.playerID, GameEndReason.TURN_TIMEOUT, "Bot didn't finish turn in time!")));
            }
            MoveBot(move, false);
            TalesOfTributeAI.Instance.move = null;
            RefreshBoard();
        } while (move.Command != CommandEnum.END_TURN);
        TalesOfTributeAI.Instance.GetLogMessages().ForEach(m => Board.Logger.Log(TalesOfTributeAI.Instance.botID, m.Item2));
        BotLogsScript.Instance.Refresh();
        
        yield return null;
    }

    void MoveBot(Move move, bool soundOn = true)
    {
        ParseMove(move, soundOn);
    }

    void ParseMove(Move move, bool soundOn)
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
                StartCoroutine(EndGame(result));
            else if(soundOn)
                cardAudio.Play();
        }
        else if (move.Command == CommandEnum.ATTACK)
        {
            var m = move as SimpleCardMove;
            var result = Board.AttackAgent(m.Card);
            if (result != null)
                StartCoroutine(EndGame(result));
        }
        else if (move.Command == CommandEnum.BUY_CARD)
        {
            var m = move as SimpleCardMove;
            var result = Board.BuyCard(m.Card);
            if (result != null)
                StartCoroutine(EndGame(result));
            else if (soundOn)
                buyCardAudio.Play();
        }
        else if (move.Command == CommandEnum.ACTIVATE_AGENT)
        {
            var m = move as SimpleCardMove;
            var result = Board.ActivateAgent(m.Card);
            if (result != null)
                StartCoroutine(EndGame(result));
        }
        else if (move.Command == CommandEnum.CALL_PATRON)
        {
            var m = move as SimplePatronMove;
            var result = Board.PatronActivation(m.PatronId);
            if (result != null)
                StartCoroutine(EndGame(result));
        }
        else if (move.Command == CommandEnum.MAKE_CHOICE)
        {
            if (move is MakeChoiceMove<UniqueCard> cardChoice)
            {
                var result = Board.MakeChoice(cardChoice.Choices);
                if (result != null)
                    StartCoroutine(EndGame(result));
            }
            else if (move is MakeChoiceMove<UniqueEffect> effectChoice)
            {
                var result = Board.MakeChoice(effectChoice.Choices.First());
                if (result != null)
                    StartCoroutine(EndGame(result));
            }
        }
    }

    

}
