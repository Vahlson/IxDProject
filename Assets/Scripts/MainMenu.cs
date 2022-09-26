using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public string firstLevel;
    public GameObject optionScreen;
    public GameObject highscoreScreen;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void startGame()
    {
        SceneManager.LoadScene(firstLevel);
    }

    public void startHighscore()
    {
        highscoreScreen.SetActive(true);
    }

    public void closeHighscore()
    {
        highscoreScreen.SetActive(false);
    }

    public void openOptions()
    {
        optionScreen.SetActive(true);

    }

    public void closeOptions()
    {
        optionScreen.SetActive(false);
    }

    public void quitGame()
    {
        Application.Quit();
        Debug.Log("Quitting");
    }
}
