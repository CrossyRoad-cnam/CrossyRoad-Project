using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    public Toggle eagleToggle;
    public Toggle robotToggle;

    // Start is called before the first frame update
    public void Start()
    {
        eagleToggle.isOn = PlayerPrefs.GetInt("Eagle", 0) == 1;
        robotToggle.isOn = PlayerPrefs.GetInt("Robot", 0) == 1;
    }

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

    public void EnabledEagle()
    {

        PlayerPrefs.SetInt("Eagle",eagleToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();

    }

    public void EnableRobot()
    {
        PlayerPrefs.SetInt("Robot", robotToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
