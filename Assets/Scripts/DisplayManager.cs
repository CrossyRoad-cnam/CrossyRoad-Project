using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class DisplayManager : MonoBehaviour
{
    [SerializeField] private Text scoreText; 
    [SerializeField] private Text highScoreText;
    [SerializeField] private Text timeText;
    [SerializeField] private Text coinText;
    private int highestScore;
    private int playerScore;
    private int coinScore;
    private ScoreManager scoreManager;
    private CoinManager coinManager;
    private bool timerStarted = false;
    private float timeSinceFirstMove = 0f;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        coinManager = FindObjectOfType<CoinManager>();
        highestScore = scoreManager.GetHighestScore();
    }

    private void Update()
    {
        DisplayScore();
        DisplayHighScore();
        DisplayTime();
        DisplayCoinScore();
    }

    public void DisplayScore()
    {
        playerScore = Mathf.RoundToInt(Player.Instance.scoreValue);
        if (scoreText != null && playerScore > 0)
        {
            scoreText.text = playerScore.ToString();
        }
    }

    public void DisplayCoinScore()
    {
        coinScore = coinManager.GetCoinScore();
        if (coinText != null && coinScore > 0)
        {
            coinText.text = "Coins: " + coinScore.ToString();
        }
    }

    public void DisplayHighScore()
    {
        if (highScoreText != null)
        {
            if (playerScore > highestScore)
                highestScore = playerScore;
        }
        highScoreText.text = "Top " + highestScore;
    }
    public void DisplayTime()
    {
       if (timeText != null && Player.Instance != null && !Player.Instance.isDead)
        {
            if (!timerStarted && Player.Instance.HasMoved())
            {
                timerStarted = true;
                timeSinceFirstMove = Time.time;
            }

            if (timerStarted)
            {
                int totalSeconds = Mathf.RoundToInt(Time.time - timeSinceFirstMove);
                int minutes = totalSeconds / 60;
                int seconds = totalSeconds % 60;
                timeText.text = string.Format("Time\n{0:D2} : {1:D2}", minutes, seconds);
            }
            else
            {
                timeText.text = "Time\n00 : 00";
            }
        }
    }
}
