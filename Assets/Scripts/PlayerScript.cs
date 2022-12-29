using System.Collections;
using System.Collections.Generic;
using TalesOfTribute;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript Instance;
    public PlayerEnum playerID { get; private set; }
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
        playerID = PlayerEnum.PLAYER1;
    }

    public void SetSide(PlayerEnum id)
    {
        this.playerID = id;
    }
}
