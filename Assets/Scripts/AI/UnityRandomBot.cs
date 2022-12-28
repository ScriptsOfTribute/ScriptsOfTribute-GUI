using SimpleBots;
using System.Collections;
using System.Collections.Generic;
using TalesOfTribute;
using UnityEngine;

public class UnityRandomBot : MonoBehaviour
{
    public static UnityRandomBot Instance { get; private set; }
    private RandomBot bot = new RandomBot();

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

    public PatronId SelectPatron(List<PatronId> availablePatrons, int round)
    {
        return bot.SelectPatron(availablePatrons, round);
    }

    public Move Play(SerializedBoard serializedBoard, List<Move> possibleMoves)
    {
        return bot.Play(serializedBoard, possibleMoves);
    }
}
