using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{

    public GameObject GameOverScreen;
    public PauseMenu pauseMenu;

    public void Awake()
    {
        GameOverScreen.SetActive(false);
    }
    public void GameOver()
    {
        GameOverScreen.SetActive(true);
        if (pauseMenu != null)
        {
            pauseMenu.enabled = false;
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
    public void QuitGame()
    {
        Destroy(Player.Instance.gameObject);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
