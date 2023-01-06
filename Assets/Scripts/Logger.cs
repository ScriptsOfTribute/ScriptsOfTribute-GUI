using System.Collections;
using System.Collections.Generic;
using TalesOfTribute;
using TalesOfTribute.Board;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public static Logger Instance;
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
    public List<CompletedAction> GetAdvancedMoves()
    {
        return GameManager.Board.GetSerializer().CompletedActions;
    }
}
