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
        // Cr�ez une nouvelle instance de ScoreData si elle n'existe pas d�j�
        if (sd == null)
        {
            sd = new ScoreData();
        }

        // R�cup�rez les scores depuis PlayerPrefs
        string json = PlayerPrefs.GetString("scores", "{}");

        // D�s�rialisez les scores depuis la cha�ne JSON
        if (!string.IsNullOrEmpty(json))
        {
            sd = JsonUtility.FromJson<ScoreData>(json);
        }

        // Affichez la cha�ne JSON pour le d�bogage
        Debug.Log("JSON: " + json);

        // Affichez les scores pour le d�bogage
        Debug.Log("Scores: " + sd.scores);
    }

    public IEnumerable<Score> GetHighScores()
    {
        return sd.scores.OrderByDescending(x => x.score);
    }

    public void AddScore(Score score)
    {
        // Initialisez la liste de scores si elle est nulle
        if (sd.scores == null)
        {
            sd.scores = new List<Score>();
        }

        // Ajoutez le score � la liste
        sd.scores.Add(score);
    }

    private void OnDestroy()
    {
        // Enregistrez les scores avant la destruction de l'objet
        SaveScore();
    }

    public void SaveScore()
    {
        // Convertissez les scores en JSON
        string json = JsonUtility.ToJson(sd);

        // Enregistrez la cha�ne JSON dans PlayerPrefs
        PlayerPrefs.SetString("scores", json);

        // Affichez la cha�ne JSON pour le d�bogage
        Debug.Log("Saved Scores: " + json);
    }
}
