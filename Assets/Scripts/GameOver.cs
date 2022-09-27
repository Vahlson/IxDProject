using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameOver : MonoBehaviour
{
    public TMP_Text score;
    public TMP_InputField playerName;
    private string _name;
    private int _score;
    private Leaderboard leaderboard;
    public GameObject defaultScreen;
    public GameObject leaderbordScreen;
    private int _placement = 1;
    [SerializeField]
    private LetterSpinner[] spinners;

    void Start()
    {
        leaderboard = DataSaver.loadData<Leaderboard>("Leaderboard");
        _score = PlayerPrefs.GetInt("Score");
        if (leaderboard != null)
        {
            foreach (var item in leaderboard.scores)
            {
                if (_score < item.score)
                {
                    _placement++;
                }
            }
        }

        score.text = "Score:" + _score.ToString() + _placement + " place";

    }

    void Update()
    {

    }
    public void SaveLetters()
    {
        _name = "";
        foreach (var item in spinners)
        {
            name += item.letter;
        }
        ShowDefault();
        LeaderboardScore newScore = new LeaderboardScore(_score, name);
        leaderboard.scores.Add(newScore);
        leaderboard.scores.Sort();
        leaderboard.scores.Reverse();
        leaderbordScreen.GetComponentInChildren<LeaderboardScreen>().CreateLeaderboardEntries(leaderboard.scores);
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
    public void ShowEnterLeaderboard()
    {
        defaultScreen.SetActive(false);
        leaderbordScreen.SetActive(true);
    }
    public void ShowDefault()
    {
        defaultScreen.SetActive(true);
        leaderbordScreen.SetActive(false);
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
