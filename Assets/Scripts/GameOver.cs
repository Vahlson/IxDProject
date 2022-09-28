using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameOver : MonoBehaviour
{
    public TMP_Text score;
    private string _name;
    private int _score;
    private Leaderboard leaderboard;
    public GameObject leaderbordScreen;
    public Toggle saveScore;
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

        score.text = "Your score:" + _score.ToString();

    }

    public void RestartGame()
    {
        if (saveScore.isOn)
        {
            SaveScore();
        }
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        if (saveScore.isOn)
        {
            SaveScore();
        }
        SceneManager.LoadScene(0);
    }


    private void SaveScore()
    {
        if (leaderboard == null)
        {
            leaderboard = new Leaderboard();
        }
        foreach (var item in spinners)
        {
            _name += item.letter;
            print(item.letter);
        }
        print(_name);
        leaderboard.scores.Add(new LeaderboardScore((int)_score, _name));
        DataSaver.saveData<Leaderboard>(leaderboard, "Leaderboard");
    }
}
