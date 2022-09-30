using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameOver : MonoBehaviour
{
    public GameObject newHighScoreScreen;
    public GameObject scoreScreen;

    void Start()
    {
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
            newHighScoreScreen.GetComponent<NewHighScoreScreen>().SaveScore();
        }
    }
}
