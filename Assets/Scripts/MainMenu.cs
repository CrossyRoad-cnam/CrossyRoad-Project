using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame() { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    // pouvoir utiliser les fleches pour choisir les boutons
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("Down");
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("Up");
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Enter");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
