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
        // Créez une nouvelle instance de ScoreData si elle n'existe pas déjà
        if (sd == null)
        {
            sd = new ScoreData();
        }

        // Récupérez les scores depuis PlayerPrefs
        string json = PlayerPrefs.GetString("scores", "{}");

        // Désérialisez les scores depuis la chaîne JSON
        if (!string.IsNullOrEmpty(json))
        {
            sd = JsonUtility.FromJson<ScoreData>(json);
        }

        // Affichez la chaîne JSON pour le débogage
        Debug.Log("JSON: " + json);

        // Affichez les scores pour le débogage
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

        // Ajoutez le score à la liste
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

        // Enregistrez la chaîne JSON dans PlayerPrefs
        PlayerPrefs.SetString("scores", json);

        // Affichez la chaîne JSON pour le débogage
        Debug.Log("Saved Scores: " + json);
    }
}
