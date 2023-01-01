using System.Collections;
using System.Collections.Generic;
using TalesOfTribute;
using TalesOfTribute.Board;
using UnityEngine;

public class MoveLogger : MonoBehaviour
{
    public static MoveLogger Instance;
    private List<Move> _simpleMoves = new List<Move>();
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

    public void AddSimpleMove(Move move)
    {
        _simpleMoves.Add(move);
    }    

    public List<Move> GetSimpleMoves()
    {
        return _simpleMoves;
    }

    public List<CompletedAction> GetAdvancedMoves()
    {
        return GameManager.Board.GetSerializer().CompletedActions;
    }
}
