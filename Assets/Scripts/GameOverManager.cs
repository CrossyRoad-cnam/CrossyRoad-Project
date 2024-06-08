using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// GESTION du canva de game Over
/// </summary>

public class GameOverManager : MonoBehaviour
{

    public GameObject GameOverScreen;
    public PauseMenu pauseMenu;
    public bool isGameOver = false;
    public void Awake()
    {
        GameOverScreen.SetActive(false);
        isGameOver = false;
    }
    public void GameOver()
    {
        GameOverScreen.SetActive(true);
        isGameOver = true;
        if (pauseMenu != null)
        {
            pauseMenu.enabled = false;
        }
    }
    public void RestartGame()
    {
        if (Player.Instance != null) { 
            Destroy(Player.Instance.gameObject);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;

    }
    public void QuitGame()
    {
        if (Player.Instance != null)
        {
            Destroy(Player.Instance.gameObject);
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
