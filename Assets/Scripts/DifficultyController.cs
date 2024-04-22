using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DifficultyController : MonoBehaviour
{

    public TextMeshProUGUI difficultyText;
    private string[] difficultyLevels = { "Easy", "Medium", "Hard" };
    private int currentDifficultyIndex = 1; // Medium par d�faut
    // Start is called before the first frame update
    void Start()
    {
        UpdateDifficultyText();
    }

    // M�thode appel�e lors du clic sur le bouton pour augmenter la difficult�
    public void IncreaseDifficulty()
    {
        currentDifficultyIndex = Mathf.Min(currentDifficultyIndex + 1, difficultyLevels.Length - 1);
        UpdateDifficultyText();
    }

    // M�thode appel�e lors du clic sur le bouton pour diminuer la difficult�
    public void DecreaseDifficulty()
    {
        currentDifficultyIndex = Mathf.Max(currentDifficultyIndex - 1, 0);
        UpdateDifficultyText();
    }

    // Met � jour le texte affichant le niveau de difficult� actuel
    private void UpdateDifficultyText()
    {
        difficultyText.text = difficultyLevels[currentDifficultyIndex];
    }
}
