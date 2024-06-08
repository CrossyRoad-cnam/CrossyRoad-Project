using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// C'est pour avoir toujours dans n'importe qu'elle scene le nombre de coin que le joueur a, ainsi que la facon de les d�penser. 
/// </summary>
public class CoinManager : MonoBehaviour
{
    private static int CoinScore
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
    private static string coinScoreKey = "coinScore";
    private static int coinScore = 0;

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

    private static void UpdateCoinScore()
    {
        PlayerPrefs.SetInt(coinScoreKey, CoinScore);    
    }

    /// <summary>
    /// Add +1 to the coinScore
    /// </summary>
    internal void CollectCoin()
    {
        AudioController audioController = GetComponent<AudioController>();
        if (audioController != null)
            audioController.Play();
        CoinScore++;
    }

    internal int GetCoinScore()
    {
        return CoinScore;
    }

    internal bool Buy(int amount)
    {
        if (CanBuy(amount))
        {
            CoinScore -= amount;
            return true;
        }
        return false;
    }

    internal bool CanBuy(int amount)
    {
        return CoinScore - amount >= 0;
    }
}
