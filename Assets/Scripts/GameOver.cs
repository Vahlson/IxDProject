using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameOver : MonoBehaviour
{
    public TMP_Text score;
    public GameObject newHighScoreScreen;
    public GameObject scoreScreen;
    private string _name = "AAA";
    private int _score;
    public GameObject leaderboardScreen;
    private int _placement = 1;
    [SerializeField]
    private LetterSpinner[] spinners;
    void Update()
    {
        if (GameManager.Instance._newLeaderboardEntry != null)
        {
            leaderboardScreen.GetComponent<LeaderboardScreen>().rotateNewHighScore(GameManager.Instance._newLeaderboardEntry);
        }
    }

    void UpdateText()
    {
        _name = "";
        foreach (var item in spinners)
        {
            _name += item.letter;
        }
        leaderboardScreen.GetComponent<LeaderboardScreen>().UpdateEntryName(_name, GameManager.Instance._newLeaderboardEntry);
        GameManager.Instance?.UpdateName(_name);


    }
    void OnDestroy()
    {
        foreach (var item in spinners)
        {
            item.GetComponent<LetterSpinner>().OnTextChanged -= UpdateText;
        }
    }
    void Start()
    {
        _score = 500;
        foreach (var item in spinners)
        {
            item.GetComponent<LetterSpinner>().OnTextChanged += UpdateText;
        }
        GameManager.Instance.CheckForHighScoreUpdates();
        leaderboardScreen.GetComponent<LeaderboardScreen>().CreateLeaderboardEntries(GameManager.Instance.getScores());
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
        foreach (var item in spinners)
        {
            _name += item.letter;
        }
        GameManager.Instance.SaveHighScore();
        // GameManager.Instance.latestName = _name;
        // leaderboard.scores.Add(_newScore);
        // DataSaver.saveData<Leaderboard>(leaderboard, "Leaderboard");
    }
}
