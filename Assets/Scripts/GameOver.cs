using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameOver : MonoBehaviour
{
    public TMP_Text score;
    public TMP_InputField playerName;
    private int _score;
    private Leaderboard leaderboard;

    void Start()
    {
        leaderboard = DataSaver.loadData<Leaderboard>("Leaderboard");
        _score = PlayerPrefs.GetInt("Score");
        score.text = "Score:" + _score.ToString();

    }

    void Update()
    {

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
        if (leaderboard == null)
        {
            leaderboard = new Leaderboard();
        }
        leaderboard.scores.Add(new LeaderboardScore((int)_score, playerName.text));
        DataSaver.saveData<Leaderboard>(leaderboard, "Leaderboard");
    }
}
