using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Vérifie si le jeu est déjà en pause
            if (Time.timeScale == 0f)
            {
                // Si oui, reprend le jeu
                ResumeGame();
            }
            else
            {
                // Si non, met le jeu en pause
                PauseGame();
            }
        }
    }
    void PauseGame()
    {
        pauseMenuUI.SetActive(true); // Active le menu pause
        Time.timeScale = 0f; // Arrête le temps dans le jeu
    }

    // Fonction pour reprendre le jeu
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Désactive le menu pause
        Time.timeScale = 1f; // Reprend le temps dans le jeu
    }

    public void QuitGame()
    {
        Time.timeScale = 0f;
        // charge la scene du menu principal
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

}
