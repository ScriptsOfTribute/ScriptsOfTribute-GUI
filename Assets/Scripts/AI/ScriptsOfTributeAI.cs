using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ScriptsOfTribute;
using ScriptsOfTribute.AI;
using ScriptsOfTribute.Serializers;
using UnityEngine;

public class ScriptsOfTributeAI : MonoBehaviour
{
    public static ScriptsOfTributeAI Instance { get; private set; }
    public PlayerEnum botID { get; private set; }
    private AI bot { get; set; }

    public string Name { get; set; }
    public bool isMoving { get; private set; }
    private TimeSpan _timeout = TimeSpan.FromMilliseconds(30000); //ms, default value
    private TimeSpan _currentTurnTimeElapsed = TimeSpan.Zero;
    public TimeSpan CurrentTurnTimeRemaining => _timeout - _currentTurnTimeElapsed;
    public Move move = null;
    private ulong _seed;
    public bool SeedSet = false;

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
        bot = null;
    }

    public void SetBotInstance(AI botInstance)
    {
        bot = botInstance;
    }

    public void SetSide(PlayerEnum id)
    {
        this.botID = id;
    }

    private Task<PatronId> SelectPatronTask(List<PatronId> availablePatrons, int round)
    {
        return Task.Run(() => bot.SelectPatron(availablePatrons, round));
    }

    private Task<Move> MoveTask(GameState serializedBoard, List<Move> possibleMoves)
    {
        return Task.Run(() => {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = bot.Play(serializedBoard, possibleMoves);
            stopwatch.Stop();
            _currentTurnTimeElapsed += stopwatch.Elapsed;
            return result;
        }
        );
    }

    public PatronId SelectPatron(List<PatronId> availablePatrons, int round)
    {
        isMoving = true;
        var task = SelectPatronTask(availablePatrons, round);
        if (task.Wait(CurrentTurnTimeRemaining))
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

    public void Play(GameState serializedBoard, List<Move> possibleMoves)
    {
        isMoving = true;
        var task = MoveTask(serializedBoard, possibleMoves);
        
        if (task.Wait(CurrentTurnTimeRemaining))
        {
            move = task.Result;
            isMoving = false;
        }
        else
        {
            move = null;
            isMoving = false;
        }
    }

    public void SetTimeout(int value)
    {
        _timeout = TimeSpan.FromMilliseconds(value);
    }

    public void SetSeed(ulong seed)
    {
        _seed = seed;
        SeedSet = true;
    }

    public void SetBotSeed()
    {
        bot.Seed = _seed;
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

    public void ResetTimer()
    {
        _currentTurnTimeElapsed = TimeSpan.Zero;
    }

}
