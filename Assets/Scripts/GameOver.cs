using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private ArduinoInputController arduinoInputController;
    public CharacterSelect characterSelect;
    public GameObject newHighScoreScreen;
    public GameObject scoreScreen;
    void Start()
    {
        this.arduinoInputController = GetComponent<ArduinoInputController>();

        GameManager.Instance.CheckForHighScoreUpdates();
        if (GameManager.Instance.IsNewHighScore())
        {
            newHighScoreScreen.SetActive(true);
            scoreScreen.SetActive(false);
        }
        else
        {
            newHighScoreScreen.SetActive(false);
            scoreScreen.SetActive(true);
        }
        characterSelect.activateCharacter(GameManager.Instance.selectedCharacter);

    }

    public void Update()
    {

        if (arduinoInputController.getKeyDown(3))
        {
            //right
            MainMenu();
        }
        if (arduinoInputController.getKeyDown(0))
        {
            //left
            RestartGame();
        }

        if (Input.GetKeyUp("d"))
        {

            MainMenu();
        }
        if (Input.GetKeyUp("a"))
        {
            RestartGame();

        }
    }

    public void RestartGame()
    {
        SaveScore();
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SaveScore();
        SceneManager.LoadScene(0);
    }


    private void SaveScore()
    {
        if (GameManager.Instance.IsNewHighScore())
        {
            GameManager.Instance.SaveHighScore();

        }
    }
}
