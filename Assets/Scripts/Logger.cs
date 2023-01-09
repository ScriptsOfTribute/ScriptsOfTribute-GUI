using System.Collections;
using System.Collections.Generic;
using TalesOfTribute;
using TalesOfTribute.Board;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public static Logger Instance;
    private List<CompletedAction> completedActions = new List<CompletedAction>();
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
