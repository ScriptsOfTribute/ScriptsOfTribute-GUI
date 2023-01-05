using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalesOfTribute;
using TalesOfTribute.AI;
using UnityEngine;

public class TalesOfTributeAI : MonoBehaviour
{
    public static TalesOfTributeAI Instance { get; private set; }
    public PlayerEnum botID { get; private set; }
    private AI bot { get; set; }
    public bool isMoving { get; private set; }
    private int _timeout = 1000; //ms, default value

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        botID = PlayerEnum.PLAYER2;
    }

    public void SetBotInstance(AI botInstance)
    {
        bot = botInstance;
    }

    public void SetSide(PlayerEnum id)
    {
        this.botID = id;
    }

    public PatronId SelectPatron(List<PatronId> availablePatrons, int round)
    {
        return bot.SelectPatron(availablePatrons, round);
    }

    public async Task<Move> Play(SerializedBoard serializedBoard, List<Move> possibleMoves)
    {
        isMoving = true;
        var task = Task.Run(() => bot.Play(serializedBoard, possibleMoves));
        if (await Task.WhenAny(task, Task.Delay(_timeout)) == task)
        {
            var move = task.Result;
            MoveLogger.Instance.AddSimpleMove(move);
            isMoving = false;
            return move;
        }
        else
        {
            return null;
        }
    }

    public void SetTimeout(int value)
    {
        _timeout = value;
    }

    public float GetTimeout()
    {
        return _timeout;
    }

    public string GetBotName()
    {
        return bot.GetType().Name;
    }

}
