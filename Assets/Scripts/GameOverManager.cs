using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{

    public GameObject GameOverScreen;

    public void Awake()
    {
        GameOverScreen.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        // Activer le canvas gameOver
        GameOverScreen.SetActive(true);
        // Arr�ter le temps (si n�cessaire)
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        // Recharger la sc�ne de jeu
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // Remettre le temps � sa valeur normale
        Time.timeScale = 1f;
    }
}
