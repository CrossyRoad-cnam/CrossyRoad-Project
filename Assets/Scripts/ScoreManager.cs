using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    private ScoreData scoreData;

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

    public int GetHighestScore()
    {
        if (scoreData.scores == null || scoreData.scores.Count == 0)
        {
            return 0;
        }
        return Mathf.RoundToInt(scoreData.scores.Max(x => x.score));
    }
}
