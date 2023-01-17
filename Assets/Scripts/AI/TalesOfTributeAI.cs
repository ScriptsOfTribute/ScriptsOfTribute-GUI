using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalesOfTribute;
using TalesOfTribute.AI;
using TalesOfTribute.Serializers;
using UnityEngine;

public class TalesOfTributeAI : MonoBehaviour
{
    public static TalesOfTributeAI Instance { get; private set; }
    public PlayerEnum botID { get; private set; }
    private AI bot { get; set; }

    public string Name { get; set; }
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

    public async Task<PatronId> SelectPatron(List<PatronId> availablePatrons, int round)
    {
        var task = Task.Run(() => bot.SelectPatron(availablePatrons, round));
        if (await Task.WhenAny(task, Task.Delay(_timeout)) == task)
        {
            var patronID = task.Result;
            if (availablePatrons.Contains(patronID))
            {
                isMoving = false;
                return patronID;
            }
        }
        return PatronId.TREASURY;
    }

    public async Task<Move> Play(GameState serializedBoard, List<Move> possibleMoves)
    {
        isMoving = true;
        var task = Task.Run(() => bot.Play(serializedBoard, possibleMoves));
        if (await Task.WhenAny(task, Task.Delay(_timeout)) == task)
        {
            var move = task.Result;
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

    public List<(DateTime, string)> GetLogMessages()
    {
        var messages = bot.LogMessages.ToList();
        bot.LogMessages.Clear();
        return messages;
    }

}
