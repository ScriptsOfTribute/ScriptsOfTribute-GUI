using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TalesOfTribute;
using TalesOfTribute.Board;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public static Logger Instance;
    private List<CompletedAction> completedActions = new List<CompletedAction>();
    public List<string> Logs;
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
        Logs = new List<string>();
    }
    public List<CompletedAction> GetMoves()
    {
        return completedActions;
    }

    public void UpdateMoves(List<CompletedAction> moves)
    {
        completedActions.Clear();
        completedActions.AddRange(moves);
    }
}

public class UnityLogStream : TextWriter
{
    private readonly Encoding _encoding = Encoding.UTF8;

    public override Encoding Encoding
    {
        get { return _encoding; }
    }

    public override void Write(string value)
    {
        Logger.Instance.Logs.Add(value);
    }
}
