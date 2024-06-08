using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// Gestion des scores du jeu 
/// </summary>
public class ScoreManager : MonoBehaviour
{
    private ScoreData scoreData;
    private int playerScore;

    void Awake()
    {
        if (scoreData == null)
        {
            scoreData = new ScoreData();
        }

        string json = PlayerPrefs.GetString("scores", "{}");

        if (!string.IsNullOrEmpty(json))
        {
            scoreData = JsonUtility.FromJson<ScoreData>(json);
        }

    }

    public void Update()
    {
        if (Player.Instance == null)
        {
            return;
        }
        playerScore = Mathf.RoundToInt(Player.Instance.scoreValue);
        if (playerScore == 0 )
        {
            return;
        }
        else if (playerScore % 50 == 0)
        {
            PlayScore();
        }
    }

    public void PlayScore()
    {
        AudioController controller = GetComponent<AudioController>();
        controller.Play();
    }
    public IEnumerable<Score> GetHighScores()
    {
        return scoreData.scores.OrderByDescending(x => x.score);
    }

    public void AddScore(Score score)
    {
        if (scoreData.scores == null)
        {
            scoreData.scores = new List<Score>();
        }

        scoreData.scores.Add(score);
    }

    private void OnDestroy()
    {
        SaveScore();
    }

    public void SaveScore()
    {
        string json = JsonUtility.ToJson(scoreData);

        PlayerPrefs.SetString("scores", json);
    }

    public IEnumerable<Score> GetScoreByDifficulty(int difficulty)
    {
        return scoreData.scores.Where(x => x.difficulty == difficulty).OrderByDescending(x => x.score);
    }

    public int GetHighestScore()
    {
        if (scoreData.scores == null || scoreData.scores.Count == 0)
        {
            return 0;
        }
        return Mathf.RoundToInt(scoreData.scores.Max(x => x.score));
    }
}
