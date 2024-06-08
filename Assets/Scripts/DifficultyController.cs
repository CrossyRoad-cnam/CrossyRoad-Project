using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// Gestion de la difficulté du jeu
/// </summary>
public class DifficultyController : MonoBehaviour
{

    public TextMeshProUGUI difficultyText;
    private string[] difficultyLevels = { "Easy", "Medium", "Hard" };
    private int currentDifficultyIndex = 1;
    private string difficultyKey = "Difficulty";

    void Start()
    {
        if (PlayerPrefs.HasKey(difficultyKey))
        {
            currentDifficultyIndex = PlayerPrefs.GetInt(difficultyKey);
        }
        UpdateDifficultyText();
    }

    public void IncreaseDifficulty()
    {
        currentDifficultyIndex = Mathf.Min(currentDifficultyIndex + 1, difficultyLevels.Length - 1);
        UpdateDifficultyText();
        SaveDifficulty();
    }

    public void DecreaseDifficulty()
    {
        currentDifficultyIndex = Mathf.Max(currentDifficultyIndex - 1, 0);
        UpdateDifficultyText();
        SaveDifficulty();
    }

    private void UpdateDifficultyText()
    {
        difficultyText.text = difficultyLevels[currentDifficultyIndex];
    }

    private void SaveDifficulty()
    {
        PlayerPrefs.SetInt(difficultyKey, currentDifficultyIndex);
        PlayerPrefs.Save();
    }
}
