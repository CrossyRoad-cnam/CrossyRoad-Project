using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    private ScoreData sd;
    private AudioSource audioSource;
    public AudioClip sound;
    private float volume = 0.5f;




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
    }

    public void PlayScore()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
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

    public int GetHighestScore()
    {
        if (sd.scores == null || sd.scores.Count == 0)
        {
            return 0;
        }
        return Mathf.RoundToInt(sd.scores.Max(x => x.score));
    }
}
