using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;

/// <summary>
/// GESTION du score
/// 
/// </summary>
public class ScoreUi : MonoBehaviour
{
    public RowUi rowUi;
    public ScoreManager scoreManager;
    public TextMeshProUGUI difficultyText;
    private int currentDifficulty;

    void Start()
    {
        currentDifficulty = PlayerPrefs.GetInt("Difficulty", 0);
        RefreshScores();
    }

    public string getDifficultyText(int difficulty)
    {
        return difficulty switch
        {
            0 => "Easy",
            1 => "Medium",
            2 => "Hard",
            _ => "Unknown",
        };
    }
    public void nextDifficulty()
    {
        if (currentDifficulty + 1 < 3)
        {
            currentDifficulty++;
            RefreshScores();

        }
    }

    public void previousDifficulty()
    {
        if (currentDifficulty - 1 >= 0)
        {
            currentDifficulty --;
            RefreshScores();
        }
    }

    void RefreshScores()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        int difficulty = currentDifficulty;
        var scores = scoreManager.GetScoreByDifficulty(difficulty).ToArray();
        difficultyText.text = getDifficultyText(difficulty);
        if (scores.Length == 0)
        {
            Debug.Log("No scores found.");
            var row = Instantiate(rowUi, transform).GetComponent<RowUi>();
            row.rank.text = "None";
            row.nameUi.text = "None";
            row.score.text = "None";
        }
        else
        {
            int length = scores.Length < 4 ? scores.Length : 4;
            for (int i = 0; i < length; i++)
            {
                var row = Instantiate(rowUi, transform).GetComponent<RowUi>();
                row.rank.text = (i + 1).ToString();
                row.nameUi.text = scores[i].name;
                row.score.text = scores[i].score.ToString();
            }
        }
    }
}
