using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    private ScoreData sd;

    void Awake()
    {
        if (sd == null)
        {
            sd = new ScoreData();
        }

        string json = PlayerPrefs.GetString("scores", "{}");

        if (!string.IsNullOrEmpty(json))
        {
            sd = JsonUtility.FromJson<ScoreData>(json);
        }

        Debug.Log("JSON: " + json);

        Debug.Log("Scores: " + sd.scores);
    }

    public IEnumerable<Score> GetHighScores()
    {
        return sd.scores.OrderByDescending(x => x.score);
    }

    public void AddScore(Score score)
    {
        if (sd.scores == null)
        {
            sd.scores = new List<Score>();
        }

        sd.scores.Add(score);
    }

    private void OnDestroy()
    {
        SaveScore();
    }

    public void SaveScore()
    {
        string json = JsonUtility.ToJson(sd);

        PlayerPrefs.SetString("scores", json);
    }
}
