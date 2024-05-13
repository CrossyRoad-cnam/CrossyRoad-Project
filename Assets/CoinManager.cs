using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private int CoinScore
    {
        get
        {
            return coinScore;
        }
        set
        {
            coinScore = value;
            UpdateCoinScore();
        }
    }
    private const string coinScoreKey = "coinScore";
    private int coinScore = 0;

    private void Start()
    {
        LoadCoinScore();
    }

    private void LoadCoinScore()
    {
        if (PlayerPrefs.HasKey(coinScoreKey))
        {
            CoinScore = PlayerPrefs.GetInt(coinScoreKey);
        }
        else
        {
            UpdateCoinScore();
            LoadCoinScore();
        }
    }

    private void UpdateCoinScore()
    {
        PlayerPrefs.SetInt(coinScoreKey, CoinScore);
    }

    /// <summary>
    /// Add +1 to the coinScore
    /// </summary>
    internal void CollectCoin()
    {
        CoinScore++;
    }

    internal int GetCoinScore()
    {
        return CoinScore;
    }
}
