using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;
    private ulong _seed;
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
        _seed = 0;
    }

    public void SetSeed(ulong seed)
    {
        _seed = seed;
    }

    public ulong GetSeed()
    {
        return _seed;
    }
}
