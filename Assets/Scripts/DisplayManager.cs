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
    private int highestScore;
    private int playerScore;
    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        highestScore = scoreManager.GetHighestScore();
        playerScore = Mathf.RoundToInt(Player.Instance.scoreValue);
    }

    private void Update()
    {
        DisplayScore();
        DisplayHighScore();
        DisplayTime();
    }

    public void DisplayScore()
    {
        if (scoreText != null)
        {
            scoreText.text = Player.Instance.scoreValue.ToString();
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
        if (timeText != null && Player.Instance != null)
        {
            int totalSeconds = Mathf.RoundToInt(Time.timeSinceLevelLoad);
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            timeText.text = "Time\n" + string.Format("{0:D2} : {1:D2}", minutes, seconds);
        }
    }
}
