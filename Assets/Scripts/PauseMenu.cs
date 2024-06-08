using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///  MENU PAUSE 
/// </summary>
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0f)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    void PauseGame()
    {
        pauseMenuUI.SetActive(true); 
        Time.timeScale = 0f; 
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; 
    }

    public void QuitGame()
    {
        Destroy(Player.Instance.gameObject);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);

    }

    public void RestartGame()
    {
        Destroy(Player.Instance.gameObject);
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

}
